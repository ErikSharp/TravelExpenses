<template>
  <div>
    <h2 class="white--text">Edit keyword</h2>
    <v-text-field
      v-model.trim="keyword.keywordName"
      :error-messages="keywordErrors"
      label="Edit name"
      box
      background-color="white"
      color="primary"
      @input="$v.keyword.$touch()"
      @blur="$v.keyword.$touch()"
    ></v-text-field>
    <v-flex xs8 offset-xs2>
      <v-layout row justify-space-around>
        <v-btn
          dark
          color="primary"
          :disabled="$v.$invalid"
          :loading="busy"
          @click="edit"
          >Edit</v-btn
        >
        <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
import { required, minLength, maxLength } from 'vuelidate/lib/validators'

const keywordMustBeUnique = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.keywordName.toLowerCase())
  return itemsLowered.indexOf(value.keywordName.toLowerCase()) < 0
}

export default {
  props: {
    keyword: Object
  },
  validations() {
    const result = {
      keyword: {
        keywordName: {
          required,
          minLength: minLength(3),
          maxLength: maxLength(255)
        },
        keywordMustBeUnique
      }
    }

    return result
  },
  methods: {
    cancel() {
      this.$emit('cancel')
    },
    edit() {
      this.$store
        .dispatch('Keyword/editKeyword', this.keyword)
        .then(() => this.$emit('cancel'))
    }
  },
  computed: {
    keywordErrors() {
      const errors = []

      if (!this.$v.keyword.$dirty) return errors

      !this.$v.keyword.keywordName.maxLength &&
        errors.push(
          `The keyword can be a maximum of ${
            this.$v.keyword.keywordName.$params.maxLength.max
          } characters`
        )
      !this.$v.keyword.keywordName.minLength &&
        errors.push(
          `The keyword must be a minimum of ${
            this.$v.keyword.keywordName.$params.minLength.min
          } characters`
        )
      !this.$v.keyword.keywordMustBeUnique &&
        errors.push('The keyword must be unique')

      !this.$v.keyword.keywordName.required &&
        errors.push('A keyword is required')
      return errors
    },
    items() {
      return this.$store.state.Keyword.keywords
    },
    busy() {
      return this.$store.state.Keyword.editKeywordBusy
    }
  },
  watch: {
    keyword() {
      this.$v.$reset()
    }
  }
}
</script>

<style scoped></style>
