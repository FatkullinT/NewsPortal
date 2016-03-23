using System.Collections.Generic;

namespace NewsPortal.Domain.Logic
{
    public interface IAuthentication
    {
        /// <summary>
        /// �������������
        /// </summary>
        void Initialize();

        /// <summary>
        /// ������ ���� OAuth-�����������
        /// </summary>
        Dictionary<string, string> OAuthProviderNames
        {
            get;
        }

        /// <summary>
        /// ���� � �������
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        bool Login(string userName, string password, bool rememberMe);

        /// <summary>
        /// ��������������� �� �������� �������������� OAuth
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="callBackUrl"></param>
        void OAuthRedirect(string providerName, string callBackUrl);

        /// <summary>
        /// ���� � ������� �� OAuth-��������������
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        bool OAuthLogin(out bool newUser);

        /// <summary>
        /// ����� �� �������
        /// </summary>
        void Logout();
    }
}