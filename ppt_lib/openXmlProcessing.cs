﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Linq.Expressions;

namespace ppt_lib
{
    internal class openXmlProcessing
    {

        public static void ProcessParagraph(Shape treeBranch, StringBuilder textBuilder)
        {
            string text = "";
            foreach (var element in treeBranch)
            {
                if (element is TextBody)
                {
                    int orderedLits=1;
                    foreach (var item in element)
                    {
                        //DocumentFormat.OpenXml.Drawing.BodyProperties
                        //DocumentFormat.OpenXml.Drawing.ListStyle
                        if (item is DocumentFormat.OpenXml.Drawing.ListStyle)
                        {

                        }
                        //DocumentFormat.OpenXml.Drawing.Paragraph -this has the size 
                        //pull the run
                        if (item is DocumentFormat.OpenXml.Drawing.Paragraph)
                        {
                            //DocumentFormat.OpenXml.Drawing.ParagraphProperties
                            DocumentFormat.OpenXml.Drawing.ParagraphProperties paragraphProperties = item.Descendants<DocumentFormat.OpenXml.Drawing.ParagraphProperties>().FirstOrDefault();
                            //could contain 

                            
                            //DocumentFormat.OpenXml.Drawing.Run run = item.Descendants<DocumentFormat.OpenXml.Drawing.Run>().FirstOrDefault();
                            DocumentFormat.OpenXml.Drawing.RunProperties runProp = item.Descendants<DocumentFormat.OpenXml.Drawing.RunProperties>().FirstOrDefault();
                            
                            

                            
                            foreach (var run in item.Descendants<DocumentFormat.OpenXml.Drawing.Run>())
                            {
                                if (run?.InnerText == null) continue;
                                DocumentFormat.OpenXml.Drawing.RunProperties props = run.RunProperties;

                                if (props.Italic != null&& props.Bold != null)
                                {
                                    text += "***" + run.InnerText.Trim() + "*** " ?? "";
                                }
                               else if (props.Italic!= null)
                                {
                                    //run.InnerText[run.InnerText.Length - 1];
                                    text += "*"+run.InnerText.Trim()+"* " ?? "";

                                }else if (props.Bold != null)
                                {
                                    text += "**" + run.InnerText.Trim() + "** " ?? "";
                                }
                                else
                                {
                                    text += run.InnerText ?? "";
                                }
                                

                            }
                            var fontSize = runProp?.FontSize ?? 0;

                            //IS A HEADER?
                            if (fontSize > 2500)
                            {
                                text = processHeader(text, fontSize);
                            }

                            //IS AUTONUMBERED LIST
                            if (isAutoNum((DocumentFormat.OpenXml.Drawing.Paragraph)item))
                            { 

                                //if last one was  auto num AND this is auto num
                                textBuilder.Append(orderedLits+" " + text + "\n");
                                text = "";
                                orderedLits++;
                                continue;
                            }
                            orderedLits = 1;
                            //IS A BULLET LIST?
                            if (isBullet((DocumentFormat.OpenXml.Drawing.Paragraph)item))
                            {
                                textBuilder.Append("*"+text + "\n");
                                text = "";
                                continue;
                            }

                            textBuilder.Append(text + "\n");

                        }


                    }
                }
            }

        }

        private static bool isBullet(DocumentFormat.OpenXml.Drawing.Paragraph paragraph)
        {
            //THE EASIEST WAY TO CHECK IF A LINE IS A BULLET-LIST IS TO CHECK THE EXISTENCE OF THIS 2 PROPERTIES 
            //INSIDE DocumentFormat.OpenXml.Drawing.ParagraphProperties SHOULD BE 
            //DocumentFormat.OpenXml.Drawing.BulletSizePercentage
            //DocumentFormat.OpenXml.Drawing.CharacterBullet
            return paragraph.Descendants<DocumentFormat.OpenXml.Drawing.CharacterBullet>().Count() > 0;
        }

        private static bool isAutoNum(DocumentFormat.OpenXml.Drawing.Paragraph paragraph)
        {
            //THE EASIEST WAY TO CHECK IF A LINE IS A BULLET-LIST IS TO CHECK THE EXISTENCE OF THIS 2 PROPERTIES 
            //INSIDE DocumentFormat.OpenXml.Drawing.ParagraphProperties SHOULD BE 
            //DocumentFormat.OpenXml.Drawing.BulletSizePercentage
            //DocumentFormat.OpenXml.Drawing.CharacterBullet
            return paragraph.Descendants<DocumentFormat.OpenXml.Drawing.AutoNumberedBullet>().Count() > 0;
        }

        public static string processHeader(string text, int fontSize=0)
        {
            
            switch (fontSize)
            {
                case >= 5500:
                    return "#" + text + "#";
                case >= 5000:
                    return "##" + text + "##";
                case >= 4500:
                    return "###" + text + "###";
                case >= 4000:
                    return "####" + text + "####";
                case >= 3500:
                    return "#####" + text + "#####";
                case >= 3000:
                    return "######" + text + "######";
                default:
                    return text;
            }
        }

    }
}
