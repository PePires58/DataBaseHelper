#region Manutenção
/*
Data: 29/02/2020
Programador: Pedro Henrique Pires
Descrição: Implementação Inicial.
*/

/*
Data: 30/03/2020
Programador: Pedro Henrique Pires
Descrição: Inclusão de campos de data.
*/

/*
Data: 31/03/2020
Programador: Pedro Henrique Pires
Descrição: Inclusão de hora, minuto e segundo em campos de data.
*/

/*
Data: 03/06/2020
Programador: Pedro Henrique Pires
Descrição: Inclusão de ajustes para valores nulos.
*/
#endregion
using DataBaseHelper.Atributos;
using DataBaseHelper.Interfaces;
using DataBaseHelper.Models;
using ModulosHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DataBaseHelper
{
    /// <summary>
    /// Classe que ajuda a montar os comandos
    /// </summary>
    internal class CommandHelper : ICommandHelper
    {
        #region Métodos
        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <returns></returns>
        StringBuilder ICommandHelper.MontaInsertPorAttributo(object pModel)
        {
            StringBuilder strBuilder = new StringBuilder();
            return MontaStringBuilderInsert(pModel, strBuilder);
        }

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <param name="pStrBuilder">String Builder</param>
        /// <returns></returns>
        StringBuilder ICommandHelper.MontaInsertPorAttributo(object pModel, ref StringBuilder pStrBuilder) =>
            MontaStringBuilderInsert(pModel, pStrBuilder);

        #endregion

        #region Métodos privados
        /// <summary>
        /// Monta o string builder para insert
        /// </summary>
        /// <param name="pModel"></param>
        /// <param name="pStrBuilder"></param>
        /// <returns></returns>
        private static StringBuilder MontaStringBuilderInsert(object pModel, StringBuilder pStrBuilder)
        {
            PropertyInfo[] propriedades = pModel.GetType().GetProperties();

            var colunaModel = new List<ColunaModel>();

            if (pModel.GetType().GetCustomAttribute(typeof(TabelaAttribute)) is TabelaAttribute pTabela)
            {

                foreach (PropertyInfo propertyInfo in propriedades)
                    if (propertyInfo.GetCustomAttribute(typeof(ColunaAttribute)) is ColunaAttribute pColuna)
                        colunaModel.Add(new ColunaModel()
                        {
                            NomeColuna = pColuna.NomeColuna,
                            TipoDadoBanco = pColuna.TipoDado,
                            ValorCampo = propertyInfo.GetValue(pModel),
                            TamanhoCampo = pColuna.TamanhoCampo
                        });

                for (int i = 0; i < colunaModel.Count; i++)
                {
                    switch (colunaModel[i].TipoDadoBanco)
                    {
                        case Enumerados.TipoDadosBanco.Enum:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} CHAR");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = '{((Enum)colunaModel[i].ValorCampo).GetDefaultValue()}'");
                            break;
                        case Enumerados.TipoDadosBanco.Char:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} CHAR");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = '{colunaModel[i]?.ValorCampo.ToString()}'");
                            break;
                        case Enumerados.TipoDadosBanco.Varchar:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} VARCHAR({colunaModel[i].TamanhoCampo})");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = {(colunaModel[i]?.ValorCampo == null ? "NULL": "'" + colunaModel[i]?.ValorCampo.ToString() + "'")}");
                            break;
                        case Enumerados.TipoDadosBanco.Tinyint:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} TINYINT");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = {(colunaModel[i].ValorCampo == null? "NULL":Convert.ToInt16(colunaModel[i].ValorCampo).ToString())}");
                            break;
                        case Enumerados.TipoDadosBanco.Integer:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} INTEGER");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = {(colunaModel[i].ValorCampo == null ? "NULL": Convert.ToInt32(colunaModel[i].ValorCampo).ToString())}");
                            break;
                        case Enumerados.TipoDadosBanco.BigInt:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} BIGINT");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = {(colunaModel[i].ValorCampo == null? "NULL": Convert.ToInt64(colunaModel[i].ValorCampo).ToString())}");
                            break;
                        case Enumerados.TipoDadosBanco.Float:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} FLOAT");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = {Convert.ToDouble(colunaModel[i].ValorCampo)}");
                            break;
                        case Enumerados.TipoDadosBanco.Byte:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} TINYINT");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = {(Convert.ToBoolean(colunaModel[i].ValorCampo) ? 1 : 0)}");
                            break;
                        case Enumerados.TipoDadosBanco.Smalldatetime:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} SMALLDATETIME");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = '{Convert.ToDateTime(colunaModel[i].ValorCampo).ToString("yyyy-MM-dd HH:mm:ss")}' ");
                            break;
                        case Enumerados.TipoDadosBanco.Datetime:
                            pStrBuilder.AppendLine($"DECLARE @{colunaModel[i].NomeColuna} DATETIME");
                            pStrBuilder.AppendLine($"SET @{colunaModel[i].NomeColuna} = '{Convert.ToDateTime(colunaModel[i].ValorCampo).ToString("yyyy-MM-dd HH:mm:ss")}' ");
                            break;
                        default:
                            break;
                    }
                }

                pStrBuilder.AppendLine();
                pStrBuilder.AppendLine($"INSERT INTO {(pTabela.Temporaria ? "#" : "")}{pTabela.NomeTabela}(");

                for (int i = 0; i < colunaModel.Count; i++)
                    pStrBuilder.AppendLine($"{colunaModel[i].NomeColuna}{(i == colunaModel.Count - 1 ? ")" : ",")}");

                pStrBuilder.AppendLine("VALUES(");

                for (int i = 0; i < colunaModel.Count; i++)
                    pStrBuilder.AppendLine($"@{colunaModel[i].NomeColuna}{(i == colunaModel.Count - 1 ? ")" : ",")}");


            }
            return pStrBuilder;
        }
        #endregion

    }
}
