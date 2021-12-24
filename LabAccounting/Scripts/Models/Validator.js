function FormValidator(form) {
    this.attachedForm = form;
    FormValidator.prototype.methods = [
        { name: ".validate-any", regex: new RegExp(".+") },
        { name: ".validate-date", regex: new RegExp("^(\\d{1}|[0-3]\\d)\\.([0]\\d{1}|[0-1][0-2])\\.\\d{4}$") },
        { name: ".validate-number", regex: new RegExp("^\\d+$") },
        { name: ".validate-radio", handler: RadioHandler }
    ];

    FormValidator.prototype.validate = function () {
        var result = true;
        var inputlist = $(form).find("input.validate");
        for (var ind = 0; ind < this.methods.length; ind++) {
            var method = this.methods[ind];
            var inputs = inputlist.filter(method.name);
            if (inputs && inputs.length > 0) {
                if (method.regex) {
                    inputs.each(function () {
                        if (method.regex.test($(this).val()) == false) {
                            $(this).parent().addClass("has-error");
                            result = false;
                        }
                        else {
                            $(this).parent().removeClass("has-error");
                        }
                    });
                }
                else if (method.handler) {
                    var tmpres = method.handler(inputs, method.selector);
                    if (tmpres)
                        result = tmpres;
                }
            }
        }
        
        return result;
    }

    function RadioHandler(elements) {
        var result = null;

        var radionames = [];
        elements.each(function () {
            var tmpname = $(this).attr("name");
            if (!radionames.find(el => el == tmpname))
                radionames.push(tmpname);
        });

        for (var ind = 0; ind < radionames.length; ind++) {
            var allrads = elements.filter("input[name='" + radionames[ind] + "']");
            var radios = allrads.filter(":checked");
            if (radios.length > 0 && radios.val()) {
                $(radios[0]).parent().parent().parent().removeClass("has-error");
            }
            else {
                $(allrads[0]).parent().parent().parent().addClass("has-error");
                result = false;
            }
        }

        return result;
    }
}

