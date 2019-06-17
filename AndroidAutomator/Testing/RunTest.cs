using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace AndroidAutomator.Testing
{

    class RunTest : CodeActivity
    {
        public InArgument<string> Filepath { get; set; }

        public FileType Type { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            switch(Type)
            {
                case FileType.Excel:
                    break;
                case FileType.JSON:
                    break;
            }
        }
    }
}
