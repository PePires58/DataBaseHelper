#region Histórico de Manutenção
/*
 Data: 15/02/2020
 Programador: Pedro Henrique Pires
 Descrição: Implementação Inicial da classe.
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
        public ColunaAttribute(string pNomeColuna, TipoDadosBanco pTipoDadosBanco)
        {
            NomeColuna = pNomeColuna;
            TipoDado = pTipoDadosBanco;
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

        #endregion

    }
}
