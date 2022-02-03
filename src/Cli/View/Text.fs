module Cli.View.Text

open Entities

/// Defines all verbs that we're using in the game that might have variations
/// depending on the gender or number. For example: for the verb have and a
/// character who's male it'd be he has, but if the character has the other
/// gender then it should be they have.
type VariableVerbs = | Have

and CommandText =
    | CommandCommonPrompt
    | CommandHelpDescription
    | CommandHelpEntry of string * Text
    | CommandDirectionDescription of direction: Direction
    | CommandLookDescription
    | CommandLookNoObjectsAround
    | CommandLookVisibleObjectsPrefix
    | CommandLookEntrances of (Direction * Text) list
    | CommandLookExit of Text
    | CommandLookObjectEntry of ObjectType * string list
    | CommandOutDescription
    | CommandExitDescription
    | CommandMapDescription
    | CommandPhoneDescription
    | CommandTalkInvalidInput
    | CommandTalkDescription
    | CommandTalkNpcNotFound of name: string
    | CommandTalkNothing

and CommonText =
    | GameName
    | CommonYouAreIn of place: string
    | CommonChoiceSelection of selection: string
    | CommonMultiChoiceMoreChoices
    | CommonMultiChoiceInstructions
    | CommonNoUnfinishedSongs
    | CommonSkills
    | CommonCancel
    | CommonBack
    | CommonBackToMainMenu
    | CommonBackToMap
    | CommonBackToPhone
    | CommonBackToWorld
    | CommonBarName
    | CommonLobbyName
    | CommonNothing
    | CommonSkillName of id: SkillId
    | CommonSkillImproved of
        characterName: string *
        characterGender: Gender *
        skill: Skill *
        previousLevel: int *
        currentLevel: int
    | CommonInvalidLength
    | CommonInvalidCommand
    | CommonDateWithDay of date: Date

and ConcertSpaceText =
    | ConcertSpaceLobbyDescription of space: ConcertSpace
    | ConcertSpaceBarDescription of space: ConcertSpace
    | ConcertSpaceStageDescription of space: ConcertSpace
    | ConcertSpaceStageName
    | ConcertSpaceStartConcert

and CreatorText =
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

and MainMenuText =
    | MainMenuIncompatibleSavegame
    | MainMenuPrompt
    | MainMenuNewGame
    | MainMenuLoadGame
    | MainMenuExit
    | MainMenuSavegameNotAvailable

and PhoneText =
    | PhoneTitle
    | PhoneOptionBank
    | PhoneOptionStatistics
    | PhonePrompt of date: Date * dayMoment: DayMoment
    | BankAppTitle
    | BankAppWelcome of characterBalance: Amount * bandBalance: Amount
    | BankAppPrompt
    | BankAppTransferToBand
    | BankAppTransferFromBand
    | BankAppTransferAmount of holder: BankAccountHolder
    | BankAppTransferSuccess of
        holder: BankAccountHolder *
        transaction: BankTransaction
    | BankAppTransferNotEnoughFunds
    | BankAppTransferNothingTransferred
    | SchedulerAssistantAppPrompt
    | SchedulerAssistantAppShow
    | SchedulerAssistantAppShowDatePrompt
    | SchedulerAssistantAppShowCityPrompt
    | SchedulerAssistantAppTicketPricePrompt
    | StatisticsAppTitle
    | StatisticsAppSectionPrompt
    | StatisticsAppSectionBand
    | StatisticsAppSectionAlbums
    | StatisticsAppBandName of name: string
    | StatisticsAppBandStartDate of date: Date
    | StatisticsAppBandFame of fame: int
    | StatisticsAppAlbumNoEntries
    | StatisticsAppAlbumName of name: string * albumType: AlbumType
    | StatisticsAppAlbumReleaseDate of date: Date
    | StatisticsAppAlbumStreams of streams: int
    | StatisticsAppAlbumRevenue of amount: Amount

and RehearsalSpaceText =
    | RehearsalSpaceRehearsalRoomName
    | RehearsalSpaceLobbyDescription
    | RehearsalSpaceBarDescription
    | RehearsalSpaceRehearsalRoomDescription
    | RehearsalRoomManageDescription
    | RehearsalRoomStatistics
    | RehearsalRoomInstrumentPlayDescription
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
    | PracticeSongItemDescription of name: string * practiceLevel: Practice
    | PracticeSongImproved of name: string * practiceLevel: Practice
    | PracticeSongAlreadyImprovedToMax of name: string
    | PracticeSongProgressLosingTime
    | PracticeSongProgressTryingSoloOnceMore
    | PracticeSongProgressGivingUp
    | ManagementTitle
    | ManagementPrompt
    | ManagementHireMember
    | ManagementFireMember
    | ManagementMemberList
    | HireMemberRolePrompt
    | HireMemberCharacterDescription of name: string * gender: Gender
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

and StudioText =
    | StudioCommonPromptReleaseAlbum of name: string
    | StudioCommonAlbumReleased of name: string
    | StudioMasteringRoomName
    | StudioMasteringRoomDescription of studio: Studio
    | StudioRecordingRoomName
    | StudioRecordingRoomDescription
    | StudioTalkIntroduction of studioName: string * fee: Amount
    | StudioTalkCreateRecord
    | StudioTalkContinueRecord
    | StudioCreateNoSongs
    | StudioCreateRecordName
    | StudioCreateTrackListPrompt
    | StudioConfirmRecordingPrompt of name: string * albumType: AlbumType
    | StudioCreateErrorNameTooShort
    | StudioCreateErrorNameTooLong
    | StudioCreateErrorNotEnoughMoney of studioBill: Amount
    | StudioCreateAlbumRecorded of albumName: string
    | StudioCreateProgressEatingSnacks
    | StudioCreateProgressRecordingWeirdSounds
    | StudioCreateProgressMovingKnobs
    | StudioContinueRecordPrompt
    | StudioContinueRecordActionPrompt
    | StudioContinueRecordActionPromptEditName
    | StudioContinueRecordActionPromptRelease
    | StudioContinueRecordAlbumRenamed of albumName: string

and WorldText =
    | WorldTitle
    | WorldStreetDescription of
        name: string *
        descriptors: OutsideNodeDescriptor list
    | WorldBoulevardDescription of
        name: string *
        descriptors: OutsideNodeDescriptor list
    | WorldSquareDescription of
        name: string *
        descriptors: OutsideNodeDescriptor list

and TextNamespace =
    | CommandText of CommandText
    | CommonText of CommonText
    | CreatorText of CreatorText
    | ConcertSpaceText of ConcertSpaceText
    | MainMenuText of MainMenuText
    | PhoneText of PhoneText
    | RehearsalSpaceText of RehearsalSpaceText
    | StudioText of StudioText
    | WorldText of WorldText

/// A reference to text inside of the CLI. Can either be a pre-defined text key
/// which resolves to the user's preferred language or a literal string that
/// always has the same value regardless of whichever language is selected.
and Text =
    | Text of TextNamespace
    | Literal of string

[<RequireQualifiedAccess>]
module I18n =
    /// Wraps a given namespace into the Text type, which when resolved by the
    /// renderer will output the translation of the given namespace and key to the
    /// currently selected language.
    let translate n = Text n

    /// Wraps a given string into the Text type as a constant, which always keeps
    /// the same value regardless of the current language.
    let constant value = Literal value
