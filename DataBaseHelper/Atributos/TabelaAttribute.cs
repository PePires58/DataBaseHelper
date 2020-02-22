#region Histórico de Manutenção
/*
 Data: 15/02/2020
 Programador: Pedro Henrique Pires
 Descrição: Implementação Inicial da classe.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseHelper.Atributos
{
    public class TabelaAttribute : Attribute
    {
        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="pNomeTabela">Nome da tabela</param>
        public TabelaAttribute(string pNomeTabela)
            :this(pNomeTabela,false)
        {

        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="pNomeTabela">Nome da tabela</param>
        /// <param name="pTemporaria">É temporária?</param>
        public TabelaAttribute(string pNomeTabela, bool pTemporaria)
        {
            NomeTabela = pNomeTabela;
            Temporaria = pTemporaria;
        }
        #endregion

        #region Propriedades
        /// <summary>
        /// Nome da tabela
        /// </summary>
        public string NomeTabela { get; set; }

        /// <summary>
        /// Valida se é temporaria
        /// </summary>
        public bool Temporaria { get; set; }
        #endregion
    }
}
