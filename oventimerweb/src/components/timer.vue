<template>    
    <div :class="timerCSSClass">
        <div class="ml-3 display-1">
            <div class="timer-label">{{ $isNullOrWhitespace(caption) ? '' : `Timer ${caption}:` }}</div>
        </div>

        <div :class="'timer-container'" @change="$emit('input', value)" no-gutters>
            <div @keypress="showSaveBtn = true">
                <span><v-text-field class="display-1 input-size inline-block" placeholder="00" :value="hour" @change="hour = $event"></v-text-field></span>        
                <span class="ml-1 display-2 inline-block">:</span>
                <span><v-text-field class="display-1 input-size inline-block" placeholder="00" :value="minute" @change="minute = $event"></v-text-field></span>        
                <span class="ml-1 display-2 inline-block">:</span>
                <span><v-text-field class="display-1 input-size inline-block" placeholder="00" :value="second" @change="second = $event"></v-text-field></span>
                <span class="inline-block" v-if="showSaveBtn"><v-btn class="ml-2 mb-2" color="success" @click="showSaveBtn = false">Save</v-btn></span>
            </div>
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
            inProcess: false,
            initValue: this.value,
            hour: '',
            minute: '',
            second: '',
            showSaveBtn: false,
            started: false,
            timer: null,
            timerCSSClass: 'timer '
        };
    },
    watch: {
        isComplete: function(val) {
            if(val) {
                this.timerCSSClass = 'timer timer-success';

                this.stopTimer();

                this.inProcess = false;
            } else {
                this.timerCSSClass = 'timer ';
            }
        },
        inProcess: function(val) {
            if(val) {
                this.timerCSSClass = 'timer timer-processing';
            }
        },
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
            this.stopTimer();

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
                    } else {
                        _self.inProcess = true;
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
            delete(this.timer);
        },
        toggle: function() {            
            this.started = !this.started;
        },
        updateService: async function() {
            let timeSet = {
                id: this.timerId.toString(),  
                hour: this.hour.toString(),
                minute: this.minute.toString(),
                second: this.second.toString()
            }

            await this.$http.patch('timer', timeSet).catch(err => {
                this.$showError(err, 'Update Timer');
            });
        }
    }
}
</script>

<style lang="scss" scoped>
@keyframes blinker {
  50% {
    opacity: 0;
  }
}

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

.timer-label {
    font-size: 24px;
}

.timer-processing {
    background-color: green;
}

.timer-success {
    animation: blinker 1s linear infinite;
    background-color: #ff0000;
}

.timer-container {
    text-align: center;
}
</style>