using System;
using System.Linq;
using System.Reflection;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Logic;
using Ninject;

namespace NewsPortal.TestDataGenerator
{
    /// <summary>
    /// Тулза для заполнения базы данных тестовыми записями
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            INewsLogic newsLogic = kernel.Get<INewsLogic>();
            for (int i = 1; i <= 100; i++)
            {
                News news = GetNewsRecord(newsLogic, i);
                newsLogic.SaveNewsRecord(news);
                Console.WriteLine("Создана запись №{0}", i);
            }
        }

        private static News GetNewsRecord(INewsLogic newsLogic, int i)
        {
            News news = newsLogic.GetEmptyNewsRecord();
            news.Date = DateTime.Now.AddHours(-i);
            news.AllowAnonymous = i%2 > 0;
            NewsText rusText = news.NewsTexts.First(text => text.Language == Language.Rus);
            rusText.Text = string.Format(@"Язык:{0}, Запись №: {1}, Доступно без регистрации:{2}
                    Разнообразный и богатый опыт постоянное информационно-пропагандистское обеспечение нашей деятельности 
                    требуют определения и уточнения систем массового участия. Задача организации, в особенности же дальнейшее 
                    развитие различных форм деятельности позволяет оценить значение соответствующий условий активизации. Идейные 
                    соображения высшего порядка, а также консультация с широким активом в значительной степени обуславливает 
                    создание соответствующий условий активизации.",
                rusText.Language,
                i,
                news.AllowAnonymous);
            NewsText engText = news.NewsTexts.First(text => text.Language == Language.Eng);
            engText.Text = string.Format(@"Language: {0}, Record number: {1}, Is available without registration: {2}
                    A varied and rich experience of constant information and propaganda support of our activity
                    require the definition and specification of systems of mass participation. The task of organizing, especially the further
                    development of various forms of activity allows us to estimate the value of the corresponding activation conditions. Ideological
                    higher-order considerations, as well as consultation with a broad asset largely determines
                    the creation of an appropriate activation conditions.",
                engText.Language,
                i,
                news.AllowAnonymous);
            return news;
        }
    }
}
