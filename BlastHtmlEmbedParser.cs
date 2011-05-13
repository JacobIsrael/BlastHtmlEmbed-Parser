//gmcs BlastHtmlEmbedParser.cs /t:library -out:../bin/BlastHtmlEmbedParser.dll
// BlastHtmlEmbedParser.cs
// 
// Copyright (C) 2008 Jacob, jacobnix@gmail.com, jcervantes@ira.cinvestav.mx
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.IO;

namespace BlastHtmlEmbedParser 
{	
 public class blastHtmlParser {
	
	private StreamReader sr;
	private StreamWriter sw;
	private string fileName;
	private string fileNew;	
	
	public void Load(string file)
	{
		fileName = Path.GetFullPath(file);
		fileNew  = Path.GetDirectoryName(fileName) + "/" + Path.GetFileNameWithoutExtension(fileName) + "_new.htm";
		sr = File.OpenText(fileName);
		sw = File.CreateText(fileNew);
	}	
	
	public void Format()
	{
		string lineHtml = "";
		string nameCtg = "";
		string anameNumber = "";
		int pos = 0;
		short countPRE = 0;		
				
		try {
			sw.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">");	
			do
			{
				/************************HEAD*******************/
				lineHtml = sr.ReadLine();
				
				if(lineHtml.IndexOf("<TITLE>") != -1) {
					sw.WriteLine(lineHtml);
					sw.WriteLine("<SCRIPT src='../../../script/drawgraphicfromBlastReport.js'></SCRIPT>");
					sw.WriteLine("<SCRIPT src='../script/drawgraphicfromBlastReport.js'></SCRIPT>");	
					continue;	
				}
					
				if(lineHtml.IndexOf("<BODY") != -1) {
					sw.WriteLine(lineHtml);
					sw.WriteLine("<form method='post' id='frmSendQueryDefFromBlast' name='frmSendQueryDefFromBlast' action='/qvisualizerDemoDeveloping.aspx'>");
					sw.WriteLine("&nbsp;&nbsp;<input id='txtqueryfblast' name='txtqueryfblast' type='text' style='visibility:hidden' >");
					sw.WriteLine("&nbsp;&nbsp;<input id='txtqueryshidden' name='txtqueryshidden' type='hidden' value=''>");
					sw.WriteLine("&nbsp;&nbsp;<input id='txtdeschidden' name='txtdeschidden' type='hidden' value=''>");	
					//sw.WriteLine("&nbsp;&nbsp;<input id='buttest' name='buttest' type='button' onclick=\"showContigInfo('E09xyz')\" />");
					sw.WriteLine("</form>");
					continue;	
				}	
					
				if(countPRE < 4) {
					sw.WriteLine(lineHtml);
					pos = lineHtml.IndexOf("PRE");
					 if(pos != -1)
						countPRE++;					
				}				
				/************************HEAD*******************/
				
				
				/************************BODY*****************/
				if(countPRE >= 4) {
					pos = lineHtml.IndexOf("<a name = ");
					 if(pos != -1) {						
					 	pos = lineHtml.IndexOf("/a");
					 	 if(pos != -1) {							
					 		nameCtg = lineHtml.Substring(pos+3,(lineHtml.Length - (pos + 3)));
					 		anameNumber = lineHtml.Substring(0,pos+3);
					 		sw.Write(anameNumber + "<span id='sp_" + nameCtg + "' style='text-decoration:underline;color:blue;cursor:pointer' onclick=\"showContigInfo('" + nameCtg + "')\">");
					 	    sw.WriteLine(nameCtg + "</span>");
					 	 }
					 }
					 else
						sw.WriteLine(lineHtml);
				}
				/************************BODY*****************/							
			}
			while(lineHtml!=null);
		}
		catch(Exception e) {
			;
		}
		
		sw.WriteLine("");sw.WriteLine("");sw.WriteLine("");
		sw.WriteLine("<b><i>Parsed by Blast Html Embed Parser Tool</i></b>");
		sw.WriteLine("<i><b>Copyright 2008</b>, <a href='mailto:jacobnix@gmail.com'>jacobnix@gmail.com</a></i>");
		sw.Close();
		File.Delete(fileName);
		File.Move(fileNew,fileName);	
	}	
}

 public class testBlastParser 
 {
	public static void Main(string[] args)
	{
		blastHtmlParser obj = new blastHtmlParser();
		obj.Load(args[0]);
		obj.Format();		
	}	
 }
}	
