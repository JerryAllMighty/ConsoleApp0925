using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp0925
{
    public class MenuChoiceException : Exception
    {
        int wrongchoice;

        public MenuChoiceException(string message) : base(message)
        {

        }

        public MenuChoiceException(int choice) : base("다시 메뉴를 선택해주세요.")
        {
            this.wrongchoice = choice;
        }
         
        public void ShowWrongChoice()
        {
            Console.WriteLine(this.wrongchoice + "에 해당하는 메뉴는 존재하지 않습니다.");
        }
    }
}
