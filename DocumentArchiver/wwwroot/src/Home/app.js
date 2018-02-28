﻿import Vue from 'vue'
import Vuex from 'vuex'
import VueRouter from 'vue-router'
import VModal from 'vue-js-modal'
import Toasted from 'vue-toasted'

import CaseListingView from './Component/CaseListingView.vue'
import appConst from '../Home/AppConst'
//import mixin from '../Home/mixin'
import store from '../Home/vuex'

Vue.use(Vuex)
//Init router
var router = new VueRouter({
    mode: 'history',
    base: 'Home',
    //root: window.location.href,
    routes: [
        { name: 'Default', path: '/', component: CaseListingView },
        { name: 'Index', path: '/Index', component: CaseListingView }
    ]
});
//Extend & reg
Vue.use(VModal, { dialog: true });
Vue.use(VueRouter);
Vue.use(Toasted,
    {
        duration: 3333,
        position: 'top-center',
        theme: 'primary',
        iconPack: 'fontawesome'
    });
//Registers globally
//Vue.mixin(mixin);
//Init
new Vue({
    //mixins: [mixin],
    store: new Vuex.Store(store),
    el: '#app',
    router: router,
    render: h => h(CaseListingView),
});