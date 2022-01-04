import { createFeatureSelector, createSelector } from '@ngrx/store';
import { State, SLICE_NAME } from './state';

const getState = createFeatureSelector<State>(SLICE_NAME);

export const getIsLoading = createSelector(getState, state => state.isLoading);
export const getDeletedPhotos = createSelector(getState, state => state.deletedPhotos);
export const getSelectedPhotosIds = createSelector(getState, state => state.selected);
export const getSelectedPhotosAmount = createSelector(getState, state => state.selected.size);
