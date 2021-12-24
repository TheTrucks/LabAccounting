function AutofillMenu(handler, datafilter, multiinput) {
    this.autoelem = null;
    this.clkhandler = handler;
    this.datafilter = datafilter;
    this.templatelist = [];
    this.multiinput = multiinput ? multiinput : [];

    AutofillMenu.prototype.RemoveMenu = function () {
        if (this.autoelem != null) {
            $(this.autoelem).remove();
            this.autoelem = null;
        }
    }

    AutofillMenu.prototype.Draw = function (event) {
        if (this.templatelist.length == 0)
            return;

        var element = event.target;
        this.RemoveMenu();

        var Elements = null;
        if (this.multiinput.length > 0) {
            var multival = [];
            multiinput.forEach(inp => multival.push($(inp).val()));
            Elements = this.FilterData(multival);
        }
        else {
            Elements = this.FilterData($(element).val());
        }

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

        var self = this;
        $(AutoMenu).find("div.auto-elem").click(function (event) {
            self.clkhandler(event.currentTarget);
            self.RemoveMenu();
        });
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
