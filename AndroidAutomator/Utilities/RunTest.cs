using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using AndroidAutomator.Activities;

namespace AndroidAutomator.Utilities
{

    class RunTest : CodeActivity
    {
        public InArgument<string> Filepath { get; set; }

        public FileType Type { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string path = Filepath.Get(context);
            switch(Type)
            {
                case FileType.Excel:
                    RunExcel(path);
                    break;
                case FileType.JSON:
                    RunJSON(path);
                    break;
            }
        }

        private void RunExcel(string path)
        {

        }

        private void RunJSON(string path)
        {

        }
    }
}
