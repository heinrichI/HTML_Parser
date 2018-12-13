using AngleSharp.Dom.Html;

namespace HTML_Parser
{
    /// <summary>
    /// Модель страницы с капчей
    /// </summary>
    public class CapchaModel
    {
        private IHtmlDocument _document;

        private string _capchaForm = ".form__inner";
        private string _formKey = ".form__key";
        private string _form_retpath = ".form__retpath";
        private string _form_trigger = ".form__trigger";
        private string _image = ".image";
        private string _captchaInput = "#rep";

        public CapchaModel(IHtmlDocument document)
        {
            _document = document;
        }

        public FieldSet GetData()
        {
            var form = _document.QuerySelector(_capchaForm);
            string formKey = _document.QuerySelector(_formKey).GetAttribute("value");
            var retpath = _document.QuerySelector(_form_retpath).GetAttribute("value");
            var img = form.QuerySelector(_image).GetAttribute("src");
            var rep = form.QuerySelector(_captchaInput).GetAttribute("name");
            return new FieldSet()
            {
                FormKey = _formKey,
                RetPath = _form_retpath,
                ImgPath = img,
                RepName = rep
            };
        }
    }
}
