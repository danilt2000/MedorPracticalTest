namespace MedorPracticalTest.Domain.Entities
{
        public class BaseEntity
        {
                public int Id { get; set; }

                public BaseEntity(int id)
                {
                        Id = id;
                }
        }
}
