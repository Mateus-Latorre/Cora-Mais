namespace AQV.WebAPI.Utils
{
    /// <summary>Utilitário de criptografia de senhas com BCrypt.</summary>
    public static class Criptografia
    {
        /// <summary>Gera o hash da senha para gravar no banco.</summary>
        public static string GerarHash(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        /// <summary>Compara a senha digitada com o hash gravado no banco.</summary>
        public static bool CompararHash(string senhaInformada, string senhaBanco)
        {
            return BCrypt.Net.BCrypt.Verify(senhaInformada, senhaBanco);
        }
    }
}