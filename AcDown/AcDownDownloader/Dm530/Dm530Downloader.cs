﻿using Kaedei.AcDown.Core;
using Kaedei.AcDown.Interface;
using Kaedei.AcDown.Interface.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Kaedei.AcDown.Interface.Downloader;

namespace Kaedei.AcDown.Downloader
{
    public class Dm530Video
    { 
        public string souce = "";
        public List<string> info = new List<string>();
    }

    public class Dm530Downloader : IDownloader
	{
		public DelegateContainer delegates { get; set; }

		public TaskInfo Info { get; set; }

		//下载参数
		DownloadParameter currentParameter = new DownloadParameter();

		//文件总长度
		public long TotalLength
		{
			get
			{
				if (currentParameter != null)
				{
					return currentParameter.TotalLength;
				}
				else
				{
					return 0;
				}
			}
		}

		//已完成的长度
		public long DoneBytes
		{
			get
			{
				if (currentParameter != null)
				{
					return currentParameter.DoneBytes;
				}
				else
				{
					return 0;
				}
			}
		}

		public bool Download()
		{
			//开始下载
			delegates.TipText(new ParaTipText(this.Info, "正在分析動畫下載位址"));

            Regex regChapter = new Regex("http://.+/v/[^-]+-(?<param1>[0-9])-(?<param2>[0-9][0-9]?).html");
            Match linkname = regChapter.Match (Info.Url);

			//要下载的Url列表
			var subUrls = new Collection<string>();

            if (linkname.Success == false)
            {
                //尼玛是整部

                regChapter = new Regex("http://www.dm530.com/show/(?<value>[^.]+).html");
                linkname = regChapter.Match(Info.Url);

                //取得Url源文件
                string src = Network.GetHtmlSource(Info.Url, Encoding.GetEncoding("GBK"), Info.Proxy);

                //填充字典
                var dict = new Dictionary<string, string>();

                int chapter_start = src.IndexOf("class=\"on\"");
                while (chapter_start > 0)
                {
                    int chapter_end = src.IndexOf("class=\"on\"", chapter_start+1);
                    if (chapter_end < 0)
                        break;

                    string sub_src = src.Substring(chapter_start, chapter_end - chapter_start);

                    regChapter = new Regex("class=\"on\">(?<source_name>[^<]+)</li>");
                    Match item = regChapter.Match(sub_src);
                    if (item.Success == false)
                        break;

                    string souce_name = item.Groups["source_name"].Value;

                    string chapter = "title='(?<chapter_name>[^']+)' href='(?<link_url>[^']+)'";
                    regChapter = new Regex(chapter);
                    item = regChapter.Match(sub_src);

                    while (item.Success)
                    {
                        string url = "http://www.dm530.com" + item.Groups["link_url"].Value;
                        string chapter_name = souce_name + "-" + item.Groups["chapter_name"].Value;
                        dict.Add(url.Trim(), chapter_name);

                        // first match is "hello world"!! but this turns out to be an infinite loop
                        item = item.NextMatch();
                    }

                    chapter_start = chapter_end;
                }

                if (dict.Count <= 0)
                    return false;

                //选择下载哪部漫画
                subUrls = ToolForm.CreateMultiSelectForm(dict, Info.AutoAnswer, "dm530");
                //如果用户没有选择任何章节
                if (subUrls.Count == 0)
                {
                    return false;
                }
            }
            else {
                subUrls.Add(Info.Url);
            }

			Info.PartCount = subUrls.Count;
			Info.CurrentPart = 0;
            foreach (string url in subUrls)
            {
                //提示更换新Part
                delegates.NewPart(new ParaNewPart(this.Info, Info.CurrentPart));

                regChapter = new Regex("http://.+/[^-]+-(?<param1>[0-9])-(?<param2>[0-9][0-9]?).html");
                Match urlname = regChapter.Match (url);

                string chaper_src = Network.GetHtmlSource(url, Encoding.GetEncoding("GBK"), Info.Proxy);

                string title_name = "NoName";
                regChapter = new Regex("<meta name=\"title\" content=\"(?<title_name>[^\"]+)\">");
                Match item = regChapter.Match(chaper_src);
                if (item.Success) {
                    title_name = item.Groups["title_name"].Value;
                }
                regChapter = new Regex("/playdata/(?<js_name>[^\"]+)");
                item = regChapter.Match(chaper_src);
                if (item.Success == false)
                    continue;

                string js_url = "http://www.dm530.com/playdata/" + item.Groups["js_name"].Value;

                string js_src = Network.GetHtmlSource(js_url, Encoding.GetEncoding("GBK"), Info.Proxy);

                int json_start = js_src.IndexOf("[");
                int json_end = js_src.LastIndexOf("]");
                js_src = js_src.Substring(json_start, json_end - json_start + 1);

                System.IO.File.WriteAllText(@"d:\dm530.txt", js_src);

                //取得视频信息
                List <Dm530Video> chapterinfo = new List<Dm530Video>();

                JArray jarr = JArray.Parse(js_src);
                int videoidx = -1;
                foreach (var jitem in jarr.Children())
                {
                    foreach (var jsubitem in jitem.Children()) {
                        if (jsubitem.Type == JTokenType.String)
                        {
                            Dm530Video video = new Dm530Video();
                            video.souce = jsubitem.ToString();
                            videoidx++;
                            chapterinfo.Add(video);
                        }
                        else if (jsubitem.Type == JTokenType.Array) {
                            chapterinfo [videoidx].info = jsubitem.ToObject<List<string>>();
                        }
                    }
                }

                int param1 = int.Parse(urlname.Groups["param1"].Value);
                int param2 = int.Parse(urlname.Groups["param2"].Value);

                DownloadChapter(chapterinfo[param1].info[param2], title_name);
            }
			return true;
		}

//        private bool InfoPaser (Dm530VideoInfo info, string js_src)
//        {
//            List<string> result = new List<string>(js_src.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
//        }

        private bool DownloadChapter(string source, string title_name)
        {
            List<string> src = new List<string>(source.Split(new[] { '$' }));
            if (src.Count <= 0)
                return false;

            string url = "";
            string web = src[src.Count - 1];
            if (web == "youku")
            {
                url = string.Format(@"http://v.youku.com/v_show/id_{0}", src[1]);
            }
            else if (web == "tudou")
            {
                List<string> sbusrc = new List<string>(src[1].Split(new[] { ',' }));
                url = string.Format(@"http://www.tudou.com/programs/view/{0}", sbusrc[1]);
            }
            else if (web == "qq")
            {
            }
            else if (web == "letv")
            {
                url = string.Format(@"http://www.letv.com/ptv/vplay/{0}.html", src[1]);
            }
            else if (web == "sohu")
            {
                url = string.Format(@"http://tv.sohu.com/20090211/{0}.shtml", src[1]);
            }
            else if (web == "cntv")
            {
                List<string> sbusrc = new List<string>(src[1].Split(new[] { '*' }));
                url = string.Format(@"http://vdn.apps.cntv.cn/api/getHttpVideoInfo.do?pid={0}", sbusrc[2]);

                string chaper_src = Network.GetHtmlSource(url, Encoding.GetEncoding("GBK"), Info.Proxy);

                Regex regChapter = new Regex("title\":\"(?<title>[^\"]+)");
                Match item = regChapter.Match(chaper_src);
                if (item.Success == false)
                    return false;

                string title = item.Groups["title"].Value;

                regChapter = new Regex("url\":\"(?<vido_url>[^\"]+)");
                List<string> partUrls = new List<string>();
                foreach (Match match in regChapter.Matches(chaper_src))
                {
                    partUrls.Add(match.Groups["vido_url"].Value);
                }

                return DownLoadFile(partUrls, title, title, "");
            }
            else if (web == "hd_iask")
            {
                url = string.Format(@"http://p.you.video.sina.com.cn/swf/bokePlayer20140424_V4_1_42_43.swf?vid={0}", src[1]);
            }
            else if (web == "flv")
            {
                List<string> partUrls = new List<string>();
                partUrls.Add (src[1]);
                DownLoadFile(partUrls, title_name, title_name + "-"+ src[0], ".flv");
            }

            if (url.Length <= 0)
                return false;
            
            //添加任务
            ParaNewTask NewTask = new ParaNewTask(new FlvcdPlugin(), url, this.Info);
            CoreManager.TaskManager.NewTaskPreprocessor (NewTask);
            return true;
        }

        public bool DownLoadFile (List<string> partUrls, string folder, string chapter, string ext)
        {
            Info.Title = chapter;
            //过滤非法字符
            folder = Tools.InvalidCharacterFilter(folder, "");

            //重新设置保存目录（生成子文件夹）
            if (!Info.SaveDirectory.ToString().EndsWith(folder))
            {
                string newdir = Path.Combine(Info.SaveDirectory.ToString(), folder);
                if (!Directory.Exists(newdir)) Directory.CreateDirectory(newdir);
                Info.SaveDirectory = new DirectoryInfo(newdir);
            }

            //清空地址
            Info.FilePath.Clear();

            //下载视频
            //确定视频共有几个段落
            Info.PartCount = partUrls.Count;

            //------------分段落下载------------
            for (int i = 0; i < Info.PartCount; i++)
            {
                Info.CurrentPart = i + 1;

                //取得文件后缀名
                if (ext.Length <= 0) {
                    ext = Tools.GetExtension(partUrls[i]);
                    if (string.IsNullOrEmpty(ext))
                    {
                        if (string.IsNullOrEmpty(Path.GetExtension(partUrls[i])))
                            ext = ".flv";
                        else
                            ext = Path.GetExtension(partUrls[i]);
                    }
                }

                string div = "";
                if (Info.PartCount > 1)
                {
                    div = "-" + string.Format("{0:00}", i);
                }
                string file_path = Path.Combine(Info.SaveDirectory.ToString(), chapter + ext);
                //设置当前DownloadParameter
                currentParameter = new DownloadParameter()
                {
                    //文件名
                    FilePath = file_path,
                    //文件URL
                    Url = partUrls[i],
                    //代理服务器
                    Proxy = Info.Proxy
                };

                //添加文件路径到List<>中
                Info.FilePath.Add(currentParameter.FilePath);
                //下载文件
                bool success;

                //提示更换新Part

                delegates.NewPart(new ParaNewPart(this.Info, i + 1));

                //下载视频
                try
                {
                    success = Network.DownloadFile(currentParameter, this.Info);
                    if (!success) //未出现错误即用户手动停止
                    {
                        return false;
                    }
                }
                catch (Exception ex) //下载文件时出现错误
                {
                    //如果此任务由一个视频组成,则报错（下载失败）
                    if (Info.PartCount == 1)
                    {
                        throw;
                    }
                    else //否则继续下载，设置“部分失败”状态
                    {
                        Info.PartialFinished = true;
                        Info.PartialFinishedDetail += "\r\n文件: " + currentParameter.Url + " 下载失败";
                    }
                }
            }

            return true;
        }


        public void StopDownload()
		{
			if (currentParameter != null)
			{
				//将停止flag设置为true
				currentParameter.IsStop = true;
			}
		}
		//最后一次Tick时的值
		public long LastTick
		{
			get
			{
				if (currentParameter != null)
				{
					//将tick值更新为当前值
					long tmp = currentParameter.LastTick;
					currentParameter.LastTick = currentParameter.DoneBytes;
					return tmp;
				}
				else
				{
					return 0;
				}
			}
		}

	}
}
