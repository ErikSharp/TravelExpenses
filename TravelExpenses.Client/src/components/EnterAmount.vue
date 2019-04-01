<template>
  <v-dialog v-model="dialog" max-width="290">
    <v-btn
      slot="activator"
      flat
      class="primary my-0"
      :class="{ title: buttonText !== 'ENTER AMOUNT' }"
      >{{ buttonText }}</v-btn
    >
    <v-card>
      <v-container grid-list-md>
        <v-text-field
          class="readout font-weight-bold"
          readonly
          solo
          :value="readout"
        ></v-text-field>
        <v-layout row wrap>
          <v-flex xs4 v-for="n in 12" :key="n">
            <v-btn
              large
              round
              :disabled="buttonDisabled(n)"
              @click="onKeyPress(n)"
              class="input px-0 font-weight-bold display-1"
              >{{ getKeyString(n) }}</v-btn
            >
          </v-flex>
        </v-layout>
      </v-container>
      <v-card-actions>
        <v-layout justify-center>
          <div>
            <v-btn @click="onBackspaceClick()" class="mr-3">
              <v-icon>backspace</v-icon>
            </v-btn>
            <v-btn dark color="primary" @click="enter()" class="ml-3"
              >ENTER</v-btn
            >
          </div>
        </v-layout>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import sum from 'lodash/sum'
import round from 'lodash/round'

export default {
  props: {
    buttonText: String
  },
  data() {
    return {
      dialog: false,
      inInt: true,
      hasValue: false,
      readout: ''
    }
  },
  methods: {
    enter() {
      if (this.readout) {
        this.evaluateAndEmitResult()
      }

      this.dialog = false
    },
    evaluateAndEmitResult() {
      let numbers = []
      let numberString = ''
      for (let i = 0; i < this.readout.length; i++) {
        let char = this.readout.charAt(i)

        if (char === '+') {
          numbers.push(+numberString)
          numberString = ''
        } else {
          numberString += char
        }
      }

      if (numberString) {
        numbers.push(+numberString)
      }

      if (numbers.length) {
        let result = round(sum(numbers), 3)
        this.$emit('amountEntered', result)
      }
    },
    onKeyPress(index) {
      const key = this.getKeyString(index)

      if (key === '+') {
        this.readout += ' + '
      } else {
        this.readout += key
      }

      this.evaluateReadout()
    },
    evaluateReadout() {
      let inInt = true
      let hasValue = false

      for (let i = 0; i < this.readout.length; i++) {
        let char = this.readout.charAt(i)
        hasValue = true

        switch (char) {
          case '.':
            inInt = false
            break
          case '+':
            inInt = true
            hasValue = false
            break
          default:
            break
        }
      }

      this.inInt = inInt
      this.hasValue = hasValue
    },
    onBackspaceClick() {
      if (
        this.readout.substring(this.readout.length - 1, this.readout.length) ===
        ' '
      ) {
        this.readout = this.readout.substring(0, this.readout.length - 3)
      } else {
        this.readout = this.readout.substring(0, this.readout.length - 1)
      }
      this.evaluateReadout()
    },
    getKeyString(index) {
      switch (index) {
        case 1:
          return '7'
        case 2:
          return '8'
        case 3:
          return '9'
        case 4:
          return '4'
        case 5:
          return '5'
        case 6:
          return '6'
        case 7:
          return '1'
        case 8:
          return '2'
        case 9:
          return '3'
        case 10:
          return '+'
        case 11:
          return '0'
        case 12:
          return '.'
      }
    },
    buttonDisabled(index) {
      const key = this.getKeyString(index)
      let result = false

      switch (key) {
        case '.':
          result = !this.inInt
          break
        case '+':
          result = !this.hasValue
          break
      }

      return result
    }
  }
}
</script>

<style scoped>
.v-btn,
.input {
  min-width: 60px;
}
</style>
