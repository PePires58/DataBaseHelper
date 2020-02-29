#region Manutenção
/*
Data: 29/02/2020
Programador: Pedro Henrique Pires
Descrição: Implementação Inicial.
*/
#endregion
using System.Text;

namespace DataBaseHelper.Interfaces
{
    /// <summary>
    /// Interface para métodos de ajuda de construção de comando
    /// </summary>
    public interface ICommandHelper
    {
        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <returns></returns>
        StringBuilder MontaInsertPorAttributo(object pModel);

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <param name="pStrBuilder">String Builder</param>
        /// <returns></returns>
        StringBuilder MontaInsertPorAttributo(object pModel, ref StringBuilder pStrBuilder);
       
    }
}
