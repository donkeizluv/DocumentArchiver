export default {
    //Does not work bc this is not in scoped
    methods: {
        startProgressBar() {
            this.$Progress.start()
        },
        setProgressBar(num) {
            this.$Progress.set(num)
        },
        increaseProgressBar(num) {
            this.$Progress.increase(num)
        },
        decreaseProgressBar(num) {
            this.$Progress.decrease(num)
        },
        finishProgressBar() {
            this.$Progress.finish()
        },
        failProgressBar() {
            this.$Progress.fail()
        },
    }
}
