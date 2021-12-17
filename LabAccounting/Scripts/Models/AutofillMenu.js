function AutofillMenu(handler, datafilter) {
    this.autoelem = null;
    this.clkhandler = handler;
    this.datafilter = datafilter;
    this.templatelist = [];

    var self = this;

    AutofillMenu.prototype.RemoveMenu = function () {
        if (this.autoelem != null) {
            $(this.autoelem).remove();
            this.autoelem = null;
        }
    }

    AutofillMenu.prototype.Draw = function (event) {
        var element = event.target;
        this.RemoveMenu();

        var Elements = this.FilterData($(element).val())
        if (Elements == null || Elements.length == 0)
            return;

        var Pos = $(element).offset();
        AutoMenu =
            $("<div>")
                .addClass("autocompletion")
                .offset({ top: Pos.top + 40, left: Pos.left });
        ;
        Elements.forEach(elem => AutoMenu.append(elem.GetAutoElem()));

        this.autoelem = AutoMenu;
        $("body").append(AutoMenu);
        $(AutoMenu).find("div.auto-elem").click(function (event) { self.ClickHandler(event); });
    }

    AutofillMenu.prototype.ClickHandler = function (event) {
        this.clkhandler(event.currentTarget);
        this.RemoveMenu();
    }

    AutofillMenu.prototype.FilterData = function (input) {
        var result = null;
        if (input.length > 0) {
            result = this.templatelist.filter(this.datafilter(input));
        }
        return result;
    }

    AutofillMenu.prototype.SetTemplates = function (items) { this.templatelist = items; }
}
