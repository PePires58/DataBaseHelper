#region Histórico de Manutenção
/*
 Data: 15/02/2020
 Programador: Pedro Henrique Pires
 Descrição: Implementação Inicial da classe.
 */

/*
Data: 29/02/2020
Programador: Pedro Henrique Pires
Descrição: Inclusão de campo de tamanho de campo.
*/
#endregion

using DataBaseHelper.Enumerados;
using System;

namespace DataBaseHelper.Atributos
{
    public class ColunaAttribute : Attribute
    {

        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="pNomeColuna">Nome da coluna</param>
        public ColunaAttribute(string pNomeColuna, TipoDadosBanco pTipoDadosBanco,int pTamanhoCampo = 0)
        {
            NomeColuna = pNomeColuna;
            TipoDado = pTipoDadosBanco;

            if (pTamanhoCampo <= 0 && (pTipoDadosBanco == TipoDadosBanco.Char || pTipoDadosBanco == TipoDadosBanco.Varchar))
                throw new ArgumentException("O tamanho do campo não pode ser igual a 0.");

            if (pTipoDadosBanco == TipoDadosBanco.Enum)
                TamanhoCampo = 1;
            else
                TamanhoCampo = pTamanhoCampo;
        }
        #endregion

        #region Propriedades

        /// <summary>
        /// Nome da coluna
        /// </summary>
        public string NomeColuna { get; set; }

        /// <summary>
        /// Tipo de dados
        /// </summary>
        public TipoDadosBanco TipoDado { get; set; }

        /// <summary>
        /// Tamanho do campo
        /// </summary>
        public int TamanhoCampo { get; set; }
        #endregion

    }
}
