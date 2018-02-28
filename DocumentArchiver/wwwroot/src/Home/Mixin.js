import appConst from '../Home/AppConst'

export default {
    computed: {
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
        }
    },
    methods: {
        HasAbility: function (name) {
            var i = this.$data.Claims[appConst.Ability].length;
            while (i--) {
                if (this.$data.Claims[appConst.Ability][i] === name) {
                    return true;
                }
            }
            return false;
        }
    },
    //data: function () {
    //    return {
    //        Claims: []
    //    }
    //}
}
