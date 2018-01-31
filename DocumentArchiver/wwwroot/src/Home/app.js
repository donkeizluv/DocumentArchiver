import Vue from 'vue'
import VueRouter from 'vue-router'
import CaseListingView from './CaseListingView.vue'
//import VModal from 'vue-js-modal'
//innit router
var router = new VueRouter({
    mode: 'history',
    base: 'Home',
    //root: window.location.href,
    routes: [
        { name: 'Default', path: '/', component: CaseListingView },
        { name: 'Index', path: '/Index', component: CaseListingView }
    ]
});

//Vue.use(VModal);
Vue.use(VueRouter);
new Vue({
    el: '#app',
    router: router,
    render: h => h(CaseListingView),
});