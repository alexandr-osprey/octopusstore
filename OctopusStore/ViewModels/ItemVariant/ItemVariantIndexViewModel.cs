﻿using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.ViewModels
{
    public class ItemVariantIndexViewModel : IndexViewModel<ItemVariantViewModel, ItemVariant>
    {
        public ItemVariantIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<ItemVariant> itemVariants)
            : base(page, totalPages, totalCount, from itemVariant in itemVariants select new ItemVariantViewModel(itemVariant))
        {  }
    }
}
