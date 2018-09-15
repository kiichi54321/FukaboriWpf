using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Text
{
    public class ReplaceText
    {
        public ReplaceText(string txt)
        {
            list.AddFirst(new TextPart(txt));
        }
        LinkedList<TextPart> list = new LinkedList<TextPart>();

        public void Replace(string txt, string replacedText)
        {
            LinkedListNode<TextPart> part = list.First;

            while (part != null)
            {
                if (part.Value.Replaced == false)
                {
                    int index = part.Value.Text.IndexOf(txt);
                    string tmpText = part.Value.Text;
                    List<TextPart> tmpList = new List<TextPart>();
                    if (index > -1)
                    {
                        if (index > 0)
                        {
                            TextPart t1 = new TextPart(tmpText.Substring(0, index ));
                            tmpList.Add(t1);
                        }
                        TextPart t2 = new TextPart(tmpText.Substring(index, txt.Length));
                        t2.Text = replacedText;
                        t2.Replaced = true;
                        tmpList.Add(t2);
                        if (tmpText.Length > index + txt.Length )
                        {
                            TextPart t3 = new TextPart(tmpText.Substring(index + txt.Length ));
                            tmpList.Add(t3);
                        }
                        LinkedListNode<TextPart> tmp = null;
                        foreach (var item in tmpList)
                        {
                            tmp = list.AddBefore(part, item);
                        }
                        list.Remove(part);
                        part = tmp;
                    }
                    else
                    {
                        part = part.Next;
                    }
                }
                else
                {
                    part = part.Next;
                }
            }
        }

        public string ToText()
        {
            StringBuilder str = new StringBuilder();
            foreach (var item in list)
            {
                str.Append(item.Text);
            }
            return str.ToString();
        }


        class TextPart
        {
            public TextPart(string txt)
            {
                this.Text = txt;
                this.Replaced = false;
            }
            public string Text { get; set; }
            public bool Replaced { get; set; }
        }
    }

}
