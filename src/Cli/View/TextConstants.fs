module Cli.View.TextConstants

open Entities

/// Defines all verbs that we're using in the game that might have variations
/// depending on the gender or number. For example: for the verb have and a
/// character who's male it'd be he has, but if the character has the other
/// gender then it should be they have.
type VariableVerbs = | Have

/// Defines all the text constants available in the application. Since this
/// might change between each UI layer (might need custom styling, etc.) the
/// Game layer simply exports these as a type that gets evaluated in each UI.
/// All types must have the screen they belong to (if any) prepended to its name.
type TextConstant =
    | GameName
    | CommonYouAreIn of place: string
    | CommonMultiChoiceMoreChoices
    | CommonMultiChoiceInstructions
    | CommonNoUnfinishedSongs
    | CommonSkills
    | CommonCancel
    | CommonBack
    | CommonBackToMainMenu
    | CommonBackToMap
    | CommonPressKeyToContinue
    | CommonSkillImproved of
        characterName: string *
        characterGender: Gender *
        skill: Skill *
        previousLevel: int *
        currentLevel: int
    | MainMenuIncompatibleSavegame
    | MainMenuPrompt
    | MainMenuNewGame
    | MainMenuLoadGame
    | MainMenuExit
    | MainMenuSavegameNotAvailable
    | CharacterCreatorInitialPrompt
    | CharacterCreatorGenderPrompt
    | CharacterCreatorGenderMale
    | CharacterCreatorGenderFemale
    | CharacterCreatorGenderOther
    | CharacterCreatorAgePrompt
    | BandCreatorInitialPrompt
    | BandCreatorGenrePrompt
    | BandCreatorInstrumentPrompt
    | BandCreatorConfirmationPrompt of
        characterName: string *
        bandName: string *
        bandGenre: string *
        instrument: string
    | CreatorErrorCharacterNameTooShort
    | CreatorErrorCharacterNameTooLong
    | CreatorErrorCharacterAgeTooYoung
    | CreatorErrorCharacterAgeTooOld
    | CreatorErrorBandNameTooShort
    | CreatorErrorBandNameTooLong
    | MapTitle
    | MapPrompt
    | MapOptionRehearsalRoom
    | MapOptionBank
    | MapOptionStudios
    | RehearsalRoomTitle
    | RehearsalRoomCompose
    | RehearsalRoomManage
    | RehearsalRoomPrompt
    | ComposePrompt
    | ComposeSong
    | ComposeSongTitlePrompt
    | ComposeSongLengthPrompt
    | ComposeSongGenrePrompt
    | ComposeSongVocalStylePrompt
    | ComposeSongConfirmation of title: string
    | ComposeSongErrorNameTooShort
    | ComposeSongErrorNameTooLong
    | ComposeSongErrorLengthTooShort
    | ComposeSongErrorLengthTooLong
    | ComposeSongProgressBrainstorming
    | ComposeSongProgressTryingChords
    | ComposeSongProgressConfiguringReverb
    | ImproveSong
    | ImproveSongSelection
    | ImproveSongCanBeFurtherImproved of
        previousQuality: Quality *
        currentQuality: Quality
    | ImproveSongReachedMaxQuality
    | ImproveSongProgressAddingSomeMelodies
    | ImproveSongProgressPlayingFoosball
    | ImproveSongProgressModifyingChordsFromAnotherSong
    | FinishSong
    | FinishSongSelection
    | FinishSongFinished of name: string * quality: Quality
    | DiscardSong
    | DiscardSongSelection
    | DiscardSongDiscarded of name: string
    | PracticeSong
    | ManagementTitle
    | ManagementPrompt
    | ManagementHireMember
    | ManagementFireMember
    | ManagementMemberList
    | HireMemberRolePrompt
    | HireMemberSkillSummary of name: string * gender: Gender
    | HireMemberSkillLine of id: SkillId * level: int
    | HireMemberConfirmation of gender: Gender
    | HireMemberHired
    | HireMemberContinueConfirmation
    | FireMemberListItem of name: string * role: InstrumentType
    | FireMemberNoMembersToFire
    | FireMemberPrompt
    | FireMemberConfirmation of name: string
    | FireMemberConfirmed of name: string
    | MemberListCurrentTitle
    | MemberListCurrentMember of
        name: string *
        role: InstrumentType *
        since: Date
    | MemberListPastTitle
    | MemberListPastMember of
        name: string *
        role: InstrumentType *
        from: Date *
        until: Date
    | BankTitle
    | BankWelcome of characterBalance: int<dd> * bandBalance: int<dd>
    | BankPrompt
    | BankTransferToBand
    | BankTransferFromBand
    | BankTransferAmount of holder: BankAccountHolder
    | BankTransferSuccess of
        holder: BankAccountHolder *
        transaction: BankTransaction
    | BankTransferNotEnoughFunds
    | StudioCommonPromptReleaseAlbum of name: string
    | StudioCommonAlbumReleased of name: string
    | StudioTitle
    | StudioWelcomePrice of name: string * price: int<dd>
    | StudioPrompt
    | StudioStartRecord
    | StudioContinueRecord
    | StudioDiscardRecord
    | StudioCreateNoSongs
    | StudioCreateRecordName
    | StudioCreateTrackListPrompt
    | StudioCreateErrorNameTooShort
    | StudioCreateErrorNameTooLong
    | StudioCreateErrorNotEnoughMoney of
        bandBalance: int<dd> *
        studioBill: int<dd>
    | StudioCreateAlbumRecorded of albumName: string
    | StudioCreateProgressEatingSnacks
    | StudioCreateProgressRecordingWeirdSounds
    | StudioCreateProgressMovingKnobs
    | StudioContinueRecordPrompt
    | StudioContinueRecordActionPrompt
    | StudioContinueRecordActionPromptEditName
    | StudioContinueRecordActionPromptRelease
    | StudioContinueRecordAlbumRenamed of albumName: string
