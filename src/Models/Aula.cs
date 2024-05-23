using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioTDD.Models
{
    public class Aula
    {
        public Aula(string titulo, TimeSpan tempoTotal)
        {   
            if(tempoTotal.CompareTo(new TimeSpan (0, 5, 0)) >= 0 )
            {
                _titulo = titulo;
                _duracao = tempoTotal;
                _tempoVisto = new TimeSpan(0, 0, 0);
                _concluida = false;
            }
            else
            {
                throw new ArgumentException("A aula não pode ter menos que cinco minutos.");
            }
            
        }
        
        private string _titulo;
        private TimeSpan _duracao;
        private TimeSpan _tempoVisto;
        private bool _concluida;

        public string Titulo
        {
            get => _titulo;

            set
            {
                bool tituloVazioOuNulo = String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value);
                
                if(tituloVazioOuNulo)
                {
                    throw new ArgumentNullException("O título não pode ser vazio");
                }

                _titulo = value;
                
            }
        }
        public TimeSpan Duracao { get => _duracao; }
        public TimeSpan TempoVisto
        {
            get => _tempoVisto;

            set
            {
                bool tempoNegativo = value.CompareTo(new TimeSpan(0, 0, 0)) < 0 || value.CompareTo(_tempoVisto) < 0;
                
                if(tempoNegativo)
                    throw new ArgumentException("O tempo visto não pode ser negativo");

                _tempoVisto = value;

                if(_tempoVisto.TotalSeconds >= (_duracao.TotalSeconds * 0.9))       // Se assistiu 90% da aula, é dita como concluída
                    _concluida = true;
            }
        }
        public bool Concluida { get => _concluida; }
    }
}