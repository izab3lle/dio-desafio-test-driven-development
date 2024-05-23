using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioTDD.Models
{
    public class Curso
    {
        public Curso(string tituloCurso)
        {
            _aulas = new List<Aula>();
            _alunos = new List<Aluno>();
            _concluido = false;
            Titulo = tituloCurso;
            Questionarios = new List<Questionario>();
        }
        
        private string _titulo;
        private bool _concluido;
        private List<Aluno> _alunos;
        private List<Aula> _aulas;
        public List<Questionario> Questionarios;
        
        public string Titulo
        {
            get => _titulo;

            set
            {
                if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("O título da aula não pode ser vazio");
                }

                _titulo = value;
            }
        }
        
        public List<Aluno> Alunos { get => _alunos; }
        
        public bool Concluido { get => _concluido; }
 
        public bool AtualizarProgresso()
        {
            foreach(Aula aula in _aulas)
            {
                if(aula.Concluida == false)
                    return _concluido;
            }

            double questionariosMedia = 0;

            foreach(Questionario questionario in Questionarios)
            {
                questionariosMedia += questionario.Resultado;
            }

            questionariosMedia /= Questionarios.Count;
            if(questionariosMedia * 100 >= 65)
                _concluido = true;
            
            return _concluido;

        }

        public Aula BuscarAula(string aulaTitulo)
        {
            foreach(Aula aula in _aulas)
            {
                if(aula.Titulo == aulaTitulo)
                    return aula;
            }
            throw new KeyNotFoundException("Aula não encontrada nesse curso");
            //return new Aula("Aula não encontrada", new TimeSpan(0, 5, 0));
        }

        public void CadastrarAula(Aula aula)
        {
            try
            {
                if(BuscarAula(aula.Titulo) == aula)
                    throw new DuplicateNameException("Já existe uma aula com esse título");
            }
            catch(KeyNotFoundException)
            {
                _aulas.Add(aula);
            }
        }

        public void RemoverAula(string titulo)
        {
            try
            {
                _aulas.Remove(BuscarAula(titulo));
            }
            catch(KeyNotFoundException)
            {
                throw;
            }
        }

        public Questionario BuscarQuestionario(Questionario questionario)
        {
            Questionario busca = Questionarios.Find(busca => busca == questionario);
            
            if(busca == null)
                throw new KeyNotFoundException("Questionário não encontrado no curso");
            
            return busca;
        }
    }
}