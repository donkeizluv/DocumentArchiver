import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'
import jwt from 'jwt-decode'



import common from '../Home/Common'

import appConst from './AppConst'
import router from './router'


import { LOGIN, LOGOUT, RELOAD_TOKEN, SET_ABILITY, SET_LAYER, SET_LAYERRANK, SET_AUTH_SUCCESS } from './actions'
import { ABILITY, LAYER, LAYERRANK, IDENTITY, EXPIRE, TOKEN } from './mutations'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        Ability: [],
        LayerName: null,
        LayerRank: -1,
        //Auth
        Token: localStorage.getItem(appConst.TokenStorage) || '',
        Identity: localStorage.getItem(appConst.IdentityStorage) || '',
        Expire: localStorage.getItem(appConst.ExpireStorage) || 0
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
        },
        AuthToken: state => state.Token,
        IsAuthenticated: state => !!state.Token,
        Identity: state => state.Identity,
    },
    mutations: {
        [ABILITY] (state, value) {
            this.state.Ability = value;
        },
        [LAYER] (state, value) {
            this.state.LayerName = value;
        },
        [LAYERRANK] (state, value) {
            this.state.LayerRank = value;
        },
        [TOKEN](state, value) {
            this.state.Token = value;
        },
        [EXPIRE](state, value) {
            this.state.Expire = value;
        },
        [IDENTITY] (state, value) {
            this.state.Identity = value;
        }
        
    },
    actions: {
        [LOGIN]: async ({ commit, dispatch }, cred) => {
                try {
                    var form = new FormData();
                    form.append('username', cred.username);
                    form.append('pwd', cred.pwd);
                    var response = await axios.post(appConst.Login, form);
                    var token = response.data.auth_token;
                    //console.log(response);
                    //Decode jwt
                    var decode = jwt(token);
                    //Store token data
                    localStorage.setItem(appConst.TokenStorage, token);
                    localStorage.setItem(appConst.IdentityStorage, cred.username);
                    localStorage.setItem(appConst.ExpireStorage, response.data.expires_in);
                    //Load states
                    dispatch(RELOAD_TOKEN);
                    //Go
                    router.push('Home');
                } catch (e) {
                    //console.log(e);
                    localStorage.removeItem(appConst.TokenStorage);
                    throw e;
                }
        },
        //Call this to init app using stored token
        [RELOAD_TOKEN]: async ({ commit, dispatch }) => {
                //Get token from store
                var token = localStorage.getItem(appConst.TokenStorage);
                var identity = localStorage.getItem(appConst.IdentityStorage);
                var exp = localStorage.getItem(appConst.ExpireStorage);
                //Check
                if (!token || !identity || exp < 1)
                    throw new Error('Fail to load token from storage');
                var decode = jwt(token);
                //Init state
                commit(TOKEN, token);
                commit(EXPIRE, exp);
                commit(IDENTITY, decode.sub);
                commit(ABILITY, decode.Ability);
                commit(LAYER, decode.LayerName);
                commit(LAYERRANK, decode.LayerRank);
                //Set token to axios
                axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;
        },
        [LOGOUT]: async ({ commit, dispatch }) => {
                //Clear all state and storage
                commit(TOKEN, '');
                commit(EXPIRE, 0);
                commit(IDENTITY,'');
                commit(ABILITY, []);
                commit(LAYER, '');
                commit(LAYERRANK, -1);
                localStorage.removeItem(appConst.TokenStorage);
                localStorage.removeItem(appConst.Identity);
                localStorage.removeItem(appConst.ExpireStorage);
                router.push('Login');
        },
        [SET_ABILITY]: ({ commit, dispatch }, value) => {
            commit(ABILITY, value);
        },
        [SET_LAYER]: ({ commit, dispatch }) => {
            commit(LAYER, value);
        },
        [SET_LAYERRANK]: ({ commit, dispatch }) => {
            commit(LAYERRANK, value);
        },
        [SET_AUTH_SUCCESS]: ({ commit, dispatch }) => {
            commit(AUTH_SUCCESS, value);
        },
    }
})