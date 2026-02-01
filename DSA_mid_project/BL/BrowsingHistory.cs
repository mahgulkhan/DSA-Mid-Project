using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_mid_project.BL
{
    public class BrowsingHistory
    {
        private string[] actions;
        private int currentPosition;
        private int maxSize;

        public BrowsingHistory()
        {
            maxSize = 30;
            actions = new string[maxSize];
            currentPosition = -1;
        }


        public void SaveAction(string actionType, int postID, string postContent = "")
        {
            if (currentPosition == maxSize - 1)
            {
                for (int i = 0; i < maxSize - 1; i++)
                {
                    actions[i] = actions[i + 1];
                }
                currentPosition--;
            }

            currentPosition++;
            string actionData = $"{actionType}|{postID}|{postContent}";
            actions[currentPosition] = actionData;
        }

        public string GetLastAction() 
        {
            if (currentPosition < 0)
                return null;

            string lastAction = actions[currentPosition];
            currentPosition--;
            return lastAction;
        }

        public bool CanUndo()
        {
            return currentPosition >= 0;
        }
    }
}
