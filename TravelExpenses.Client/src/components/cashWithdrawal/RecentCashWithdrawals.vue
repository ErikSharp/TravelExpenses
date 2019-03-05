<template>
  <div>
    <v-dialog v-model="memoDialog" max-width="290">
      <v-card>
        <v-card-title class="headline">Memo</v-card-title>
        <v-card-text>{{ memo }}</v-card-text>
        <v-card-actions>
          <v-btn color="primary" @click="memoDialog = false">CLOSE</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <div
      class="helper-text"
      v-if="
        !Object.keys(recentCashWithdrawals).length && !recentCashWithdrawalsBusy
      "
    >
      <h1 class="text-xs-center white--text">No Cash Withdrawals</h1>
      <p class="text-xs-center white--text">
        Press the Add button below to create your first.
      </p>
    </div>
    <v-layout
      v-show="
        !Object.keys(recentCashWithdrawals).length && recentCashWithdrawalsBusy
      "
      justify-center
    >
      <v-progress-circular
        class="mt-5"
        size="70"
        width="7"
        color="secondary"
        indeterminate
      ></v-progress-circular>
    </v-layout>
    <v-expansion-panel dark v-model="panel" expand>
      <v-expansion-panel-content
        class="primary mt-1"
        v-for="(dateGroup, date) in recentCashWithdrawals"
        :key="date"
      >
        <div slot="header">{{ getDateString(date) }}</div>
        <div style="background: #261136" class="py-1 px-2">
          <cash-withdrawal-card
            v-for="(cashWithdrawal, i) in dateGroup"
            :key="i"
            :cashWithdrawal="cashWithdrawal"
            @showMemo="showMemo($event)"
          />
        </div>
      </v-expansion-panel-content>
    </v-expansion-panel>
    <v-layout justify-center>
      <v-btn
        v-show="Object.keys(recentCashWithdrawals).length"
        flat
        :loading="recentCashWithdrawalsBusy"
        :disabled="noMoreCashWithdrawals"
        class="primary"
        @click="loadMore"
        >Load More</v-btn
      >
    </v-layout>
    <div class="bottom-spacer"></div>
    <v-flex class="button-background" xs12>
      <v-flex xs12 sm10 offset-sm1>
        <v-layout justify-center justify-space-between>
          <v-btn flat class="primary my-3" @click="addCashWithdrawal">
            <v-icon dark>add</v-icon>Add
          </v-btn>
          <v-btn
            flat
            :disabled="!cashWithdrawalSelected"
            class="primary my-3"
            @click="editCashWithdrawal"
          >
            <v-icon dark>edit</v-icon>Edit
          </v-btn>
          <v-dialog
            v-model="deleteDialog"
            :disabled="!cashWithdrawalSelected"
            persistent
            max-width="290"
          >
            <v-btn
              slot="activator"
              flat
              :disabled="!cashWithdrawalSelected"
              class="primary my-3"
            >
              <v-icon dark>delete</v-icon>Delete
            </v-btn>
            <v-card>
              <v-avatar size="70" class="mx-2 elevation-5 red">
                <v-icon size="45" class="white--text">warning</v-icon>
              </v-avatar>
              <v-card-title class="headline"
                >Delete Cash Withdrawal</v-card-title
              >
              <v-card-text
                >There is no way to undo this procedure. Do you wish to
                proceed?</v-card-text
              >
              <v-card-actions>
                <v-layout justify-space-around>
                  <v-btn
                    color="red"
                    :loading="saveCashWithdrawalBusy"
                    dark
                    @click="deleteCashWithdrawal"
                    >YES</v-btn
                  >
                  <v-btn
                    color="primary"
                    :disabled="saveCashWithdrawalBusy"
                    @click="deleteDialog = false"
                    >NO</v-btn
                  >
                </v-layout>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-layout>
      </v-flex>
    </v-flex>
  </div>
</template>

<script>
/* eslint-disable no-console */
import groupBy from 'lodash/groupBy'
import CashWithdrawalCard from '@/components/cashWithdrawal/CashWithdrawalCard.vue'
import { mapState } from 'vuex'

const dateOptions = {
  weekday: 'long',
  year: 'numeric',
  month: 'short',
  day: '2-digit'
}

const locale = navigator.language || 'en-US'

export default {
  components: {
    CashWithdrawalCard
  },
  data() {
    return {
      panel: [true],
      deleteDialog: false,
      memoDialog: false,
      memo: ''
    }
  },
  methods: {
    showMemo(memo) {
      this.memo = memo
      this.memoDialog = true
    },
    loadMore() {
      this.$store.dispatch('CashWithdrawal/getNextCashWithdrawals')
    },
    addCashWithdrawal() {
      this.$emit('addCashWithdrawal')
    },
    editCashWithdrawal() {
      this.$emit('editCashWithdrawal')
    },
    deleteCashWithdrawal() {
      this.$store.dispatch(
        'CashWithdrawal/deleteSelectedCashWithdrawal',
        () => {
          this.deleteDialog = false
        }
      )
    },
    getDateString(date) {
      return new Date(date).toLocaleDateString(locale, dateOptions)
    }
  },
  computed: {
    ...mapState('CashWithdrawal', [
      'recentCashWithdrawalsBusy',
      'noMoreCashWithdrawals',
      'saveCashWithdrawalBusy'
    ]),
    recentCashWithdrawals() {
      return groupBy(
        this.$store.state.CashWithdrawal.recentCashWithdrawals,
        t => t.transDate
      )
    },
    cashWithdrawalSelected() {
      return !!this.$store.state.CashWithdrawal.selectedCashWithdrawal.title
    }
  },
  watch: {
    recentCashWithdrawals(val) {
      this.panel = new Array(Object.keys(val).length)
      this.panel.fill(false)

      if (this.panel.length) {
        this.panel[0] = true
      }
    }
  }
}
</script>

<style scoped>
.button-background {
  position: fixed;
  width: 100%;
  bottom: 56px;
  background: rgba(0, 0, 0, 0.8);
}

.bottom-spacer {
  height: 79px;
}

.helper-text {
  margin-top: 75px;
}
</style>
