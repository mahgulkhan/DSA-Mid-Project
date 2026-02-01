using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA_mid_project
{
    public static class NavigationStack
    {
        private static NSstackDL stack = new NSstackDL();

        public static void Push(Form form)
        {
            FormDataBL formData = new FormDataBL(form);
            stack.Push(formData);
        }

        public static FormDataBL Pop()
        {
            return stack.Pop();
        }

        public static void Clear()
        {
            stack.Clear();
        }
        public static bool IsEmpty()
        {
            return stack.IsEmpty();
        }
    }
    public class NSnodeBL
    {
        public FormDataBL Data { get; set; }
        public NSnodeBL Next { get; set; }

        public NSnodeBL(FormDataBL data)
        {
            Data = data;
            Next = null;
        }
    }
    internal class NSstackDL
    {
        private NSnodeBL top;

        public NSstackDL()
        {
            top = null;
        }

        public bool IsEmpty()
        {
            return top == null;
        }

        public void Push(FormDataBL item)
        {
            NSnodeBL newNode = new NSnodeBL(item);
            newNode.Next = top;
            top = newNode;
        }

        public FormDataBL Pop()
        {
            if (IsEmpty())
            {
                MessageBox.Show("Stack Underflow!");
                return null;
            }

            NSnodeBL temp = top;
            FormDataBL item = temp.Data;
            top = top.Next;
            return item;
        }
        
        public void Clear()
        {
            top = null;
        }
    }
}