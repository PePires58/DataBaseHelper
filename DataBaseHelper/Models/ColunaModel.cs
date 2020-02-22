#region Histórico de Manutenção
/*
 Data: 15/02/2020
 Programador: Pedro Henrique Pires
 Descrição: Implementação Inicial da classe.
 */
#endregion

using DataBaseHelper.Enumerados;

namespace DataBaseHelper.Models
{
    /// <summary>
    /// Classe de auxílio
    /// </summary>
    internal class ColunaModel
    {

        #region Propriedades

        /// <summary>
        /// Nome das colunas
        /// </summary>
        internal string NomeColuna { get; set; }


        /// <summary>
        /// Tipo de dados
        /// </summary>
        internal TipoDadosBanco TipoDadoBanco { get; set; }

        /// <summary>
        /// Valor do campo
        /// </summary>
        internal object ValorCampo { get; set; }
        #endregion
    }
}
