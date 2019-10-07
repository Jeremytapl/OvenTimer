<template>    
    <div :class="isComplete ? 'timer timer-success' : 'timer'">
        <div class="ml-3">
            <v-label>{{ $isNullOrWhitespace(caption) ? '' : `Timer ${caption}:` }}</v-label>
        </div>

        <div :class="'timer-container'" @change="$emit('input', value)" no-gutters>
            <span><v-text-field class="display-1 input-size inline-block" placeholder="00" readonly :value="hour"></v-text-field></span>        
            <span class="ml-1 display-2 inline-block">:</span>
            <span><v-text-field class="display-1 input-size inline-block" placeholder="00" readonly :value="minute"></v-text-field></span>        
            <span class="ml-1 display-2 inline-block">:</span>
            <span><v-text-field class="display-1 input-size inline-block" placeholder="00" readonly :value="second"></v-text-field></span>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        caption: null,
        timerId: {
            required: true,
            type: String
        },
        value: {
            required: true,
            type: String
        }
    },    
    created() {        
        this.parseValue();
    },
    data() {
        return {     
            isComplete: false,
            initValue: this.value,
            hour: '',
            minute: '',
            second: '',
            started: false,
            timer: null
        };
    },
    watch: {
        started: function(val) {
            if(val) {
                this.startTimer();
            } else {
                this.stopTimer();
            }
        },
        value: function(val) {
            this.initValue = val;

            this.parseValue();
        }
    }, 
    methods: {
        parseValue: function() {
            if(this.initValue != null) {
                let splitter = this.initValue.toString().split(':');

                if(splitter.length == 3) {
                    this.hour = splitter[0];
                    this.minute = splitter[1];
                    this.second = splitter[2];
                }
            }
        },        
        reset: function() {
            this.isComplete = false;
            this.parseValue();            
        },
        startTimer: function() {
            if(this.timer == null) {
                let hourInt = Number.parseInt(this.hour);
                let minuteInt = Number.parseInt(this.minute);
                let secondInt = Number.parseInt(this.second);

                let _self = this;
                this.timer = setInterval(function() {                    
                    // Stop when timer has run out
                    if(hourInt == 0 && minuteInt == 0 && secondInt == 0) {                        
                        _self.isComplete = true;
                        clearInterval(_self.timer);

                        return;
                    }

                    if(hourInt > 0 && minuteInt == 0) {
                        hourInt--;
                    }

                    if(minuteInt > 0 && secondInt == 0) {
                        minuteInt--;
                    } else if(hourInt != 0 && minuteInt == 0 && secondInt == 0) {
                        minuteInt = 59;
                    }

                    if(secondInt > 0) {
                        secondInt--;
                    } else if(secondInt == 0) {
                        secondInt = 59;
                    }

                    _self.hour = _self.$getFormatTime(hourInt);
                    _self.minute = _self.$getFormatTime(minuteInt);
                    _self.second = _self.$getFormatTime(secondInt);

                    _self.updateService();
                }, 1000);
            }
        },
        stopTimer: function() {
            clearInterval(this.timer);

            this.timer = null;
        },
        toggle: function() {            
            this.started = !this.started;
        },
        updateService: async function() {
            let timeSet = {
                id: this.timerId,  
                hour: this.hour,
                minute: this.minute,
                second: this.second
            }

            await this.$http.patch('timer', timeSet).catch(err => {
                this.$showError(err, 'Update Timer');
            });
        }
    }
}
</script>

<style lang="scss" scoped>
.inline-block {
    display: inline-block;
}

.input-size {
    width: 40px;
}

.timer {
    background-color: #424242; 
    border-radius: 15px;
    margin: 5px; 
    padding: 5px;
}

.timer-success {
    background-color: #1B5E20;
}

.timer-container {
    text-align: center;
}
</style>