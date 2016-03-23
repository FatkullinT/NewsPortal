using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Logic
{
    public interface IAccountLogic
    {
        /// <summary>
        /// ��������� �������� �������� ������������
        /// </summary>
        /// <returns></returns>
        Account GetAccount();

        /// <summary>
        /// ���������� �������� �������� ������������
        /// </summary>
        /// <param name="accountRecord"></param>
        void SaveAccount(Account accountRecord);
    }
}