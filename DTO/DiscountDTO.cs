using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class DiscountDTO
    {
        public long Id { get; set; } // Идентификатор скидки
        public long? Size { get; set; } // Размер скидки
        public long? Start { get; set; } // Начало диапазона суммы покупок
        public long End { get; set; } // Конец диапазона суммы покупок

        // Конструктор для инициализации на основе сущности discount
        public DiscountDTO(discount discountEntity)
        {
            if (discountEntity == null)
                throw new ArgumentNullException(nameof(discountEntity));

            Id = discountEntity.id;
            Size = discountEntity.size;
            Start = discountEntity.start;
            End = discountEntity.end;
        }
    }

}
