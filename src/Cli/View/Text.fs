module Cli.View.Text

open Entities

/// Defines all verbs that we're using in the game that might have variations
/// depending on the gender or number. For example: for the verb have and a
/// character who's male it'd be he has, but if the character has the other
/// gender then it should be they have.
type VariableVerbs = | Have

type BankText =
    | BankTitle
    | BankWelcome of characterBalance: Amount * bandBalance: Amount
    | BankPrompt
    | BankTransferToBand
    | BankTransferFromBand
    | BankTransferAmount of holder: BankAccountHolder
    | BankTransferSuccess of
        holder: BankAccountHolder *
        transaction: BankTransaction
    | BankTransferNotEnoughFunds

and CommandText =
    | CommandCommonPrompt
    | CommandHelpDescription
    | CommandHelpEntry of string * Text
    | CommandDirectionDescription of direction: Direction
    | CommandLookDescription
    | CommandLookNoObjectsAround
    | CommandLookVisibleObjectsPrefix
    | CommandLookEntranceDescription of (Direction * Text) list
    | CommandLookObjectEntry of ObjectType * string list
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
    | CommonPressKeyToContinue
    | CommonSkillImproved of
        characterName: string *
        characterGender: Gender *
        skill: Skill *
        previousLevel: int *
        currentLevel: int
    | CommonInvalidLength
    | CommonInvalidCommand

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
    | PhonePrompt

and RehearsalSpaceText =
    | RehearsalSpaceLobbyName
    | RehearsalSpaceBarName
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

and StatisticsText =
    | StatisticsTitle
    | StatisticsSectionPrompt
    | StatisticsSectionBand
    | StatisticsSectionAlbums
    | StatisticsBandName of name: string
    | StatisticsBandStartDate of date: Date
    | StatisticsBandFame of fame: int
    | StatisticsAlbumNoEntries
    | StatisticsAlbumName of name: string * albumType: AlbumType
    | StatisticsAlbumReleaseDate of date: Date
    | StatisticsAlbumStreams of streams: int
    | StatisticsAlbumRevenue of amount: Amount

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
    | StreetBoringDescription of name: string

and TextNamespace =
    | BankText of BankText
    | CommandText of CommandText
    | CommonText of CommonText
    | CreatorText of CreatorText
    | MainMenuText of MainMenuText
    | PhoneText of PhoneText
    | RehearsalSpaceText of RehearsalSpaceText
    | StatisticsText of StatisticsText
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
