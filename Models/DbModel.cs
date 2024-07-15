using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApi.Models
{
    public class DbModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public double value { get; set; }
        public DbModel modeltransform(ModelForUser model)
        {
            return new DbModel
            {
                question=model.question,
                answer=model.answer,
                value=model.value
            };
        }


    }
}
