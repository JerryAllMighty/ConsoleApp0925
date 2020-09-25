using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp0925
{
    public class PhoneBookManager
    {
        const int MAX_CNT = 100;
        PhoneInfo[] infoStorage = new PhoneInfo[MAX_CNT];
        int curCnt = 0;

        public void ShowMenu()
        {
            Console.WriteLine("------------------------ 주소록 -----------------------------------");
            Console.WriteLine("1. 입력  |  2. 목록  |  3. 검색  |  4. 정렬  |  5. 삭제  |  6. 종료");
            Console.WriteLine("------------------------------------------------------------------");
            Console.Write("선택: ");
        }

        public void SortData()
        {
            int choice;
            while (true)
            {
                try
                {
                    Console.WriteLine("1.이름 ASC  2.이름 DESC  3.전화번호 ASC  4.전화번호 DESC");
                    Console.Write("선택 >> ");

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        if (choice < 1 || choice > 4)
                        {
                            throw new MenuChoiceException(choice);

                            //Console.WriteLine("1.이름 ASC  2.이름 DESC  3.전화번호 ASC  4.전화번호 DESC 중에 선택하십시오.");
                            //return;
                        }
                        else
                            break;
                    }
                }
                catch (MenuChoiceException err)
                {
                    err.ShowWrongChoice();
                }
            }

            PhoneInfo[] new_arr = new PhoneInfo[curCnt];
            Array.Copy(infoStorage, new_arr, curCnt);

            if (choice == 1)
            {
                Array.Sort(new_arr, new NameComparator());
            }
            else if (choice == 2)
            {
                Array.Sort(new_arr, new NameComparator());
                Array.Reverse(new_arr);
            }
            else if (choice == 3)
            {
                Array.Sort(new_arr, new PhoneComparator());
            }
            else if (choice == 4)
            {
                Array.Sort(new_arr, new PhoneComparator());
                Array.Reverse(new_arr);
            }

            for (int i = 0; i < curCnt; i++)
            {
                Console.WriteLine(new_arr[i].ToString());
            }
        }

        public void InputData()
        {
            Console.WriteLine("1.일반  2.대학  3.회사");
            Console.Write("선택 >> ");
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice))
                    break;
            }
            if (choice < 1 || choice > 3)
            {
                Console.WriteLine("1.일반  2.대학  3.회사 중에 선택하십시오.");
                return;
            }

            PhoneInfo info = null;
            switch (choice)
            {
                case 1:
                    info = InputFriendInfo();
                    break;
                case 2:
                    info = InputUnivInfo();
                    break;
                case 3:
                    info = InputCompanyInfo();
                    break;
            }
            if (info != null)
            {
                infoStorage[curCnt++] = info;
                Console.WriteLine("데이터 입력이 완료되었습니다");
            }
        }

        private string[] InputCommonInfo()
        {
            Console.Write("이름: ");
            string name = Console.ReadLine().Trim();
            //if (name == "") or if (name.Length < 1) or if (name.Equals(""))
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("이름은 필수입력입니다");
                return null;
            }
            else
            {
                int dataIdx = SearchName(name);
                if (dataIdx > -1)
                {
                    Console.WriteLine("이미 등록된 이름입니다. 다른 이름으로 입력하세요");
                    return null;
                }
            }

            Console.Write("전화번호: ");
            string phone = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(phone))
            {
                Console.WriteLine("전화번호는 필수입력입니다");
                return null;
            }

            Console.Write("생일: ");
            string birth = Console.ReadLine().Trim();

            string[] arr = new string[3];
            arr[0] = name;
            arr[1] = phone;
            arr[2] = birth;

            return arr;
        }

        private PhoneInfo InputFriendInfo()
        {
            string[] comInfo = InputCommonInfo();
            if (comInfo == null || comInfo.Length != 3)
                return null;

            return new PhoneInfo(comInfo[0], comInfo[1], comInfo[2]);
        }

        private PhoneInfo InputUnivInfo()
        {
            string[] comInfo = InputCommonInfo();
            if (comInfo == null || comInfo.Length != 3)
                return null;

            Console.Write("전공: ");
            string major = Console.ReadLine().Trim();

            Console.Write("학년: ");
            int year = int.Parse(Console.ReadLine().Trim());

            return new PhoneUnivInfo(comInfo[0], comInfo[1], comInfo[2], major, year);
        }

        private PhoneInfo InputCompanyInfo()
        {
            string[] comInfo = InputCommonInfo();
            if (comInfo == null || comInfo.Length != 3)
                return null;

            Console.Write("회사명: ");
            string company = Console.ReadLine().Trim();

            return new PhoneCompanyInfo(comInfo[0], comInfo[1], comInfo[2], company);
        }

        public void ListData()
        {
            if (curCnt == 0)
            {
                Console.WriteLine("입력된 데이터가 없습니다.");
                return;
            }

            for (int i = 0; i < curCnt; i++)
            {
                //infoStorage[i].ShowPhoneInfo();
                //Console.WriteLine();

                Console.WriteLine(infoStorage[i].ToString());
            }
        }

        public void SearchData()
        {
            Console.WriteLine("주소록 검색을 시작합니다......");
            int dataIdx = SearchName();
            if (dataIdx < 0)
            {
                Console.WriteLine("검색된 데이터가 없습니다");
            }
            else
            {
                infoStorage[dataIdx].ShowPhoneInfo();
                Console.WriteLine();
            }

            #region 모두 찾기
            //int findCnt = 0;
            //for(int i=0; i<curCnt; i++)
            //{
            //    // ==, Equals(), CompareTo()
            //    if (infoStorage[i].Name.Replace(" ","").CompareTo(name) == 0)
            //    {
            //        infoStorage[i].ShowPhoneInfo();
            //        findCnt++;
            //    }
            //}
            //if (findCnt < 1)
            //{
            //    Console.WriteLine("검색된 데이터가 없습니다");
            //}
            //else
            //{
            //    Console.WriteLine($"총 {findCnt} 명이 검색되었습니다.");
            //}
            #endregion
        }

        private int SearchName()
        {
            Console.Write("이름: ");
            string name = Console.ReadLine().Trim().Replace(" ", "");

            for (int i = 0; i < curCnt; i++)
            {
                if (infoStorage[i].Name.Replace(" ", "").CompareTo(name) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        private int SearchName(string name)
        {
            for (int i = 0; i < curCnt; i++)
            {
                if (infoStorage[i].Name.Replace(" ", "").CompareTo(name) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public void DeleteData()
        {
            Console.WriteLine("주소록 삭제를 시작합니다......");

            int dataIdx = SearchName();
            if (dataIdx < 0)
            {
                Console.WriteLine("삭제할 데이터가 없습니다");
            }
            else
            {
                for (int i = dataIdx; i < curCnt; i++)
                {
                    infoStorage[i] = infoStorage[i + 1];
                }
                curCnt--;
                Console.WriteLine("주소록 삭제가 완료되었습니다");
            }
        }
    }
}
