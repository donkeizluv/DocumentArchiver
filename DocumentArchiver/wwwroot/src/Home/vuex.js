import Vuex from 'vuex'
import common from '../Home/Common'
import appConst from '../Home/AppConst'

export default {
    state: {
        Ability: [],
        LayerName: null,
        LayerRank: -1
    },
    getters: {
        CanUpdate: state => {
            return common.arrayContains(state.Ability, appConst.CanUpdateClaim);
        },
        CanDelete: state => {
            return common.arrayContains(state.Ability, appConst.CanDeleteClaim);
        },
        CanCreate: state => {
            return common.arrayContains(state.Ability, appConst.CanCreateClaim);
        },
        CanDownload: state => {
            return common.arrayContains(state.Ability, appConst.CanDownloadClaim);
        }
    },
    mutations: {
        Ability(state, value) {
            this.state.Ability = value;
        },
        LayerName(state, value) {
            this.state.LayerName = value;
        },
        LayerRank(state, value) {
            this.state.LayerRank = value;
        }
    }
}