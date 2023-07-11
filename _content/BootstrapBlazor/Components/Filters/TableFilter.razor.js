﻿import Data from "../../modules/data.js?v=7.8.4"
import EventHandler from "../../modules/event-handler.js?v=7.8.4"
import Popover from "../../modules/base-popover.js?v=7.8.4"

export function init(id) {
    const el = document.getElementById(id)
    if (el == null) {
        return;
    }

    const config = {
        class: 'popover-dropdown',
        toggleClass: '[data-bs-toggle="bb.dropdown"]',
        dropdownSelector: '.filter-item',
        isDisabled: () => {
            return false
        }
    }
    const action = el.querySelector(config.dropdownSelector)
    const dismissSelector = el.getAttribute('data-bb-dismiss')
    const dropdown = Popover.init(el, config)

    const tableFilter = {
        el,
        action,
        dismissSelector,
        dropdown
    }
    Data.set(id, tableFilter)

    const buttons = el.querySelectorAll(dismissSelector)
    EventHandler.on(action, 'click', dismissSelector, () => {
        dropdown.popover.hide()
    })
    EventHandler.on(el, 'keyup', e => {
        if (e.key === 'Escape') {
            buttons.item(0).click()
        }
        else if (e.key === 'Enter') {
            buttons.item(1).click()
        }
    });

    // hack input in modal
    tableFilter.body = el.querySelector('.card-body')
    EventHandler.on(tableFilter.body, 'focusin', 'input', e => {
        e.stopPropagation()
    })
}

export function dispose(id) {
    const tableFilter = Data.get(id)
    Data.remove(id)

    if (tableFilter) {
        EventHandler.off(tableFilter.action, 'click', tableFilter.dismissSelector)
        Popover.dispose(tableFilter.dropdown)
        EventHandler.off(tableFilter.body, 'focusin')
    }
}