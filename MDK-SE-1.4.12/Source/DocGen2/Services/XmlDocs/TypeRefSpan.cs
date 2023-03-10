using System.Threading.Tasks;
using Mal.DocGen2.Services.Markdown;

namespace Mal.DocGen2.Services.XmlDocs
{
    class TypeRefSpan : Span
    {
        public TypeRefSpan(string textValue) : base(textValue)
        { }

        public override async Task WriteMarkdown(XmlDocWriteContext context, MarkdownWriter writer)
        {
            await writer.WriteAsync(" ");
            var entry = context.ResolveReference(TextValue);
            if (entry.Key == null)
                await writer.WriteAsync(entry.Value ?? TextValue);
            else
                await writer.WriteAsync(MarkdownInline.HRef(entry.Value, entry.Key));
            await writer.WriteAsync(" ");
        }
    }
}