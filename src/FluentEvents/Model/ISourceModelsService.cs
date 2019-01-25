﻿using System;
using System.Collections.Generic;

namespace FluentEvents.Model
{
    public interface ISourceModelsService
    {
        SourceModel GetSourceModel(Type crlType);
        SourceModel GetOrCreateSourceModel(Type clrType);
        IEnumerable<SourceModel> GetSourceModels();
    }
}