﻿using System;
using System.IO;
using System.Web.Mvc;

namespace BeyondThemes.Bootstrap.Controls
{
    public class CarouselCustomItem : IDisposable
    {
        private readonly TextWriter textWriter;
        private readonly UrlHelper urlHelper;

        internal CarouselCustomItem(TextWriter writer, UrlHelper urlHelper, bool isFirstItem)
        {
            this.textWriter = writer;
            this.urlHelper = urlHelper;
            if (isFirstItem)
            {
                this.textWriter.Write(@"<div class=""item active"">");
            }
            else
            {
                this.textWriter.Write(@"<div class=""item"">");
            }
        }

        public void Dispose()
        {
            this.textWriter.Write("</div>");
        }
    }
}
