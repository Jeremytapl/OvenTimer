export default {
    methods: {              
        $getFormatTime: function(time) {
            if(time < 10) {
                time = '0' + time;
            }

            return time;
        },
        $isNullOrWhitespace(val) {
            let isNullOrWhitespace =
                val == null || val.toString().match(/^ *$/) !== null;

            return isNullOrWhitespace;
        },
        $showError(errorText, errorTitle) {            
            this.$root.$emit('error', errorText, errorTitle);
        }
    }
};