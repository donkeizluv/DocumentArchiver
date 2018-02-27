export default {
    methods: {
        CanUpdate: function () {
            return this.HasAbility(appConst.CanUpdateClaim);
        },
        CanDelete: function () {
            return this.HasAbility(appConst.CanDeleteClaim);
        },
        CanCreate: function () {
            return this.HasAbility(appConst.CanCreateClaim);
        },
        CanDownload: function () {
            return this.HasAbility(appConst.CanDownloadClaim);
        },
        HasAbility: function (name) {
            var i = this.$data.Ability.length;
            while (i--) {
                if (this.$data.Ability[i] === name) {
                    return true;
                }
            }
            return false;
        }
    },
    data: function () {
        return {
            Ability: []
        }
    }
}
