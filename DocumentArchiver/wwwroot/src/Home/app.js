﻿import Vue from 'vue'
import VueRouter from 'vue-router'
import CaseListingView from './CaseListingView.vue'
import VModal from 'vue-js-modal'
import Toasted from 'vue-toasted';

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
        iconPack: 'fontawesome'});
//Init
new Vue({
    el: '#app',
    router: router,
    render: h => h(CaseListingView),
});