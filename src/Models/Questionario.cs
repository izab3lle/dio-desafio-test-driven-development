using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioTDD.Models
{
    public class Questionario
    {
        public Questionario()
        {
            _questoes = new List<string>();
            _respostas = new List<string>();
            _historicoResultados = new List<double>();
        }

        private List<string> _questoes;
        private List<string> _respostas;
        private double _resultado;
        private List<double> _historicoResultados;

        public double Resultado { get => _resultado; }
        public string ResultadoPorcentagem { get => $"{_resultado}%"; }
        public List<string> HistoricoResultados
        {
            get
            {
                List<string> resultadoPorcentagem = new List<string>();
                foreach(double resultado in _historicoResultados)
                {
                    resultadoPorcentagem.Add($"{resultado}%");
                }

                return resultadoPorcentagem;
            }
        }

         public void AdicionarQuestao(string questao, string resposta)
        {
            if(_questoes.Contains(questao))
                throw new ArgumentException("Essa pergunta já foi cadastrada");

            _questoes.Add(questao);
            _respostas.Add(resposta);
        }

        public void RemoverQuestao(string questao)
        {
            if(_questoes.Contains(questao))
            {
                _questoes.Remove(questao);
            }
            else
            {
                throw new KeyNotFoundException("Questão não encontrada");
            }
        }

        public void AtualizarHistorico()
        {
            _historicoResultados.Add(_resultado);
        }

        public void Responder(string[] respostas)
        {
            if(respostas.Length < _respostas.Count)
                throw new ArgumentException("A quantidade de respostas é insuficiente");
            
            double acertos = 0;
            double totalQuestoes = _questoes.Count;
            
            for(int i = 0; i < totalQuestoes; i++)
            {
                if(respostas[i] == _respostas[i])
                    acertos++;
            }

            _resultado = acertos / totalQuestoes * 100;
            AtualizarHistorico();
        }
    }
}